using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

public class AnthropicProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<AnthropicProvider> logger) : ILLMProviderAdapter
{
    public string ProviderType => "Anthropic";

    public async Task<string> GetResponse(
        string providerId,
        LLMProviderSettings provider,
        string model,
        LLMRequestDto request,
        CancellationToken ct)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Post,
            HttpLLMProviderHelper.BuildUri(provider, "v1/messages"));
        message.Headers.Add("x-api-key", provider.ApiKey);
        message.Headers.Add("anthropic-version", "2023-06-01");
        message.Content = JsonContent.Create(new
        {
            model,
            max_tokens = 8192,
            system = "You must return ONLY valid JSON. No markdown. No commentary.",
            messages = new[]
            {
                new { role = "user", content = request.Prompt }
            }
        });

        var client = httpClientFactory.CreateClient();
        client.Timeout = HttpLLMProviderHelper.RequestTimeout;

        using var response = await client.SendAsync(message, ct);
        var payload = await HttpLLMProviderHelper.ReadResponse(response, ProviderType, ct);
        var result = ExtractText(payload);

        logger.LogDebug(
            "LLM interaction. Provider={Provider}, Model={Model}, Prompt={RequestPrompt}, Response={Response}",
            providerId, model, request.Prompt, result);

        return result;
    }

    private static string ExtractText(string payload)
    {
        using var document = JsonDocument.Parse(payload);
        foreach (var content in document.RootElement.GetProperty("content").EnumerateArray())
        {
            if (content.GetProperty("type").GetString() == "text")
                return content.GetProperty("text").GetString()
                       ?? throw new BusinessException("Anthropic returned an empty response.");
        }

        throw new BusinessException("Anthropic returned no text content.");
    }
}
