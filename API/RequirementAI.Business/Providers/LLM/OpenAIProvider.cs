using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Exceptions;

namespace RequirementAI.Business.Providers.LLM;

public class OpenAIProvider : ILLMProvider
{
    private readonly ChatClient _chat;
    private readonly ILogger<OpenAIProvider> _logger;

    public OpenAIProvider(IConfiguration config, 
        ILogger<OpenAIProvider> logger)
    {
         var client = new OpenAIClient(config["OpenAI:ApiKey"]);
        _chat = client.GetChatClient(config["OpenAI:Model"]);
        _logger = logger;
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

        var response = completion.Value.Content[0].Text ?? throw new BusinessException("LLM Request failed.");
        
        _logger.LogDebug(
            "LLM interaction. Prompt={Prompt}, Response={Response}",
            request.Prompt,
            response);
        
        return response;
    }
}