using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Services;

public class UserStoryService(ILLMProvider llmProvider) : IUserStoryService
{
    public async Task<UserStoryDto> Generate(string description, CancellationToken ct)
    {
        return await llmProvider.Generate<UserStoryDto>(description, ct);
    }
}