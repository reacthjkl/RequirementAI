using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;

namespace RequirementAI.Business.Providers.LLM;

public class OpenAIProvider : ILLMProvider
{
    private readonly ChatClient _chat;

    public OpenAIProvider(IConfiguration config)
    {
         var client = new OpenAIClient(config["OpenAI:ApiKey"]);
        _chat = client.GetChatClient(config["OpenAI:Model"]);
    }
    public async Task<string> GetResponse(LLMRequestDto request, CancellationToken ct)
    {
        var completion = await _chat.CompleteChatAsync(
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
        
        return completion.Value.Content[0].Text
                   ?? throw new BusinessException("LLM Request failed.");
    }
}