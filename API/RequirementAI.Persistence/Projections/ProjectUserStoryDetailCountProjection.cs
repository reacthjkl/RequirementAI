namespace RequirementAI.Persistence.Projections;

public class ProjectUserStoryDetailCountProjection
{
    public Guid ProjectId { get; init; }
    public int UserStoryCount { get; init; }
    public int TotalAcceptanceCriteria { get; init; }
    public int TotalEdgeCases { get; init; }
}
