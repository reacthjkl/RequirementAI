namespace RequirementAI.Contract.Dto;

public class PersonaQualityScoreDto: QualityScoreBaseDto
{
    public Guid PersonaId { get; set; }

    public decimal ClarityScore { get; set; }
    public decimal RealismScore { get; set; }
    public decimal GoalClarityScore { get; set; }
    public decimal PainPointsScore { get; set; }
    public decimal RelevanceScore { get; set; }
    public decimal DifferentiationScore { get; set; }
}
