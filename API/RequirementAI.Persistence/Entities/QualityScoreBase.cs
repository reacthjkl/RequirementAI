namespace RequirementAI.Persistence.Entities;

public abstract class QualityScoreBase: BaseEntity
{
    public int OverallScore { get; set; }
    
    public string Strengths { get; set; } = null!;
    public string Weaknesses { get; set; } = null!;
    public string Suggestions { get; set; } = null!;
}