namespace RequirementAI.Persistence.Entities;

public class UserStoryQualityScore: QualityScoreBase
{
    public Guid UserStoryId { get; set; }
    public UserStory UserStory { get; set; } = null!;

    public int ClarityScore { get; set; }
    public int CompletenessScore { get; set; }
    public int TestabilityScore { get; set; }
    public int AcceptanceCriteriaScore { get; set; }
    public int ScopeScore { get; set; }
    public int BusinessValueScore { get; set; }
    public int AmbiguityScore { get; set; }
}