namespace RequirementAI.Persistence.Entities;

public class PersonaQualityScore: QualityScoreBase
{
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;

    public decimal ClarityScore { get; set; }
    public decimal RealismScore { get; set; }
    public decimal GoalClarityScore { get; set; }
    public decimal PainPointsScore { get; set; }
    public decimal RelevanceScore { get; set; }
    public decimal DifferentiationScore { get; set; }
}
