using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.Refinement;

public interface IUserStorySplitService
{
    Task<IReadOnlyList<UserStory>> SplitUserStory(
        UserStory userStory,
        string? customInstructions,
        CancellationToken ct);
}
