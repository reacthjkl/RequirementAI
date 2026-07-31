namespace RequirementAI.Contract.Dto;

public abstract class QualityScoreBaseDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public decimal OverallScore { get; set; }
    
    public string Strengths { get; set; } = null!;
    public string Weaknesses { get; set; } = null!;
    public string Suggestions { get; set; } = null!;
}
