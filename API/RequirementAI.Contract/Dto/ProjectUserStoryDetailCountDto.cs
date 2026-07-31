namespace RequirementAI.Contract.Dto;

public class ProjectUserStoryDetailCountDto
{
    public Guid ProjectId { get; set; }
    public int UserStoryCount { get; set; }
    public int TotalAcceptanceCriteria { get; set; }
    public int TotalEdgeCases { get; set; }
    public decimal AverageAcceptanceCriteriaPerUserStory { get; set; }
    public decimal AverageEdgeCasesPerUserStory { get; set; }
}
