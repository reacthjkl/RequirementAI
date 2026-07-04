using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

public class OpenAIProvider(ILogger<OpenAIProvider> logger) : ILLMProviderAdapter
{
    private readonly ConcurrentDictionary<(string Provider, string Model), ChatClient> _clients = new();

    public string ProviderType => "OpenAI";

    public async Task<string> GetResponse(
        string providerId,
        LLMProviderSettings provider,
        string model,
        LLMRequestDto request,
        CancellationToken ct)
    {
        var chat = _clients.GetOrAdd(
            (providerId, model),
            _ => new OpenAIClient(provider.ApiKey).GetChatClient(model));

        var completion = await chat.CompleteChatAsync(
            new List<ChatMessage>
            {
                new SystemChatMessage("You must return ONLY valid JSON. No markdown. No commentary."),
                new UserChatMessage(request.Prompt)
            },
            new ChatCompletionOptions
            {
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            },
            
            cancellationToken: ct);

        var response = completion.Value.Content[0].Text ?? throw new BusinessException("LLM Request failed.");
        
        logger.LogInformation(
            "LLM interaction. Prompt={RequestPrompt}, Response={Response}", request.Prompt, response);

        return response;
    }
}
