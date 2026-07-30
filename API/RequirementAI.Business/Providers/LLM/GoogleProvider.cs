using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

public class GoogleProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<GoogleProvider> logger) : ILLMProviderAdapter
{
    public string ProviderType => "Google";

    public async Task<string> GetResponse(
        string providerId,
        LLMProviderSettings provider,
        string model,
        LLMRequestDto request,
        CancellationToken ct)
    {
        var client = httpClientFactory.CreateClient();
        client.Timeout = HttpLLMProviderHelper.RequestTimeout;

        using var message = CreateRequestMessage(provider, model, request);

        using var response = await client.SendAsync(message, ct);

        var payload = await HttpLLMProviderHelper.ReadResponse(
            response,
            ProviderType,
            ct);

        var result = ExtractText(payload);

        logger.LogInformation(
            "LLM interaction. Provider={Provider}, Model={Model}, Prompt={RequestPrompt}, Response={Response}",
            providerId, model, request.Prompt, result);

        return result;
    }

    private static string ExtractText(string payload)
    {
        using var document = JsonDocument.Parse(payload);
        var parts = document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts");
        var result = new StringBuilder();

        foreach (var part in parts.EnumerateArray())
            if (part.TryGetProperty("text", out var text))
                result.Append(text.GetString());

        return result.Length > 0
            ? result.ToString()
            : throw new BusinessException("Google returned no text content.");
    }

    private static HttpRequestMessage CreateRequestMessage(
        LLMProviderSettings provider,
        string model,
        LLMRequestDto request)
    {
        var message = new HttpRequestMessage(
            HttpMethod.Post,
            HttpLLMProviderHelper.BuildUri(
                provider,
                $"models/{Uri.EscapeDataString(model)}:generateContent"));

        message.Headers.Add("x-goog-api-key", provider.ApiKey);

        message.Content = JsonContent.Create(new
        {
            systemInstruction = new
            {
                parts = new[]
                {
                    new
                    {
                        text = "You must return ONLY valid JSON. No markdown. No commentary."
                    }
                }
            },
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = request.Prompt }
                    }
                }
            },
            generationConfig = new
            {
                maxOutputTokens = 8192
            }
        });

        return message;
    }
}
