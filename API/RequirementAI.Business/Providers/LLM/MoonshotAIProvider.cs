using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

public class MoonshotAIProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<MoonshotAIProvider> logger) : ILLMProviderAdapter
{
    public string ProviderType => "MoonshotAI";

    public async Task<string> GetResponse(
        string providerId,
        LLMProviderSettings provider,
        string model,
        LLMRequestDto request,
        CancellationToken ct)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Post,
            HttpLLMProviderHelper.BuildUri(provider, "chat/completions"));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.ApiKey);
        message.Content = JsonContent.Create(new
        {
            thinking = new
            {
                type = "disabled"
            },
            model,
            max_completion_tokens = 8192,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You must return ONLY valid JSON. No markdown. No commentary."
                },
                new { role = "user", content = request.Prompt }
            },
            response_format = new { type = "json_object" }
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
        return document.RootElement
                   .GetProperty("choices")[0]
                   .GetProperty("message")
                   .GetProperty("content")
                   .GetString()
               ?? throw new BusinessException("Moonshot AI returned an empty response.");
    }
}
