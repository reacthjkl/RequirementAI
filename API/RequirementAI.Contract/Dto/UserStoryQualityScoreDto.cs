namespace RequirementAI.Contract.Dto;

public class UserStoryQualityScoreDto: QualityScoreBaseDto
{
    public Guid UserStoryId { get; set; }

    public int ClarityScore { get; set; }
    public int CompletenessScore { get; set; }
    public int TestabilityScore { get; set; }
    public int AcceptanceCriteriaScore { get; set; }
    public int ScopeScore { get; set; }
    public int BusinessValueScore { get; set; }
    public int AmbiguityScore { get; set; }
}