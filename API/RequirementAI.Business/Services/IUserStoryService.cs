using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Services;

public interface IUserStoryService
{
    Task<UserStoryDto> Generate(string description, CancellationToken ct);
}

