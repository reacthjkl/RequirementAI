using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Exceptions;

namespace RequirementAI.Business.Providers.LLM;

public class OpenAIProvider : ILLMProvider
{
    private readonly ChatClient _chat;
    private readonly IPromptProvider _promptProvider;

    public OpenAIProvider(IConfiguration config, IPromptProvider promptProvider)
    {
         var client = new OpenAIClient(config["OpenAI:ApiKey"]);
        _chat = client.GetChatClient(config["OpenAI:Model"]);
        
        _promptProvider = promptProvider;
    }
    public async Task<T> Generate<T>(string description, CancellationToken ct)
    {
        var request = _promptProvider.Build<T>(description);
        
        var completion = await _chat.CompleteChatAsync(
            new List<ChatMessage>
            {
                new SystemChatMessage("You are a strict JSON generator."),
                new UserChatMessage(request.Prompt)
            }, cancellationToken: ct);
        
        var text = completion.Value.Content[0].Text
            ?? throw new BusinessException("LLM Request failed.");
        
        return JsonSerializer.Deserialize<T>(text)
            ?? throw new BusinessException("LLM Request failed.");
    }
}