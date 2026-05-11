using RequirementAI.Business.Helpers;
using RequirementAI.Business.Services;
using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public class UserStoryService(ILLMProvider llmProvider) : IUserStoryService
{
    public async Task<UserStoryDto> Generate(string description, CancellationToken ct)
    {
        return await llmProvider.Generate<UserStoryDto>(description, ct);
    }
}