namespace RequirementAI.Persistence.Entities;

public class UserStoryQualityScore: QualityScoreBase
{
    public Guid UserStoryId { get; set; }
    public UserStory UserStory { get; set; } = null!;

    public decimal ClarityScore { get; set; }
    public decimal CompletenessScore { get; set; }
    public decimal TestabilityScore { get; set; }
    public decimal AcceptanceCriteriaScore { get; set; }
    public decimal ScopeScore { get; set; }
    public decimal BusinessValueScore { get; set; }
    public decimal AmbiguityScore { get; set; }
}
