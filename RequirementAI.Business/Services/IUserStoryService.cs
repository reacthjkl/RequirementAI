using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Services;

public interface IUserStoryService
{
    Task<List<UserStoryDto>> Generate(string description, CancellationToken ct);
}

