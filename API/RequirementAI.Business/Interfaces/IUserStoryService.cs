using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IUserStoryService
{
    Task<UserStoryDto> Generate(string description, CancellationToken ct);
}

