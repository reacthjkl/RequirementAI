namespace RequirementAI.Persistence.Entities;

public class PersonaQualityScore: QualityScoreBase
{
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;

    public int ClarityScore { get; set; }
    public int RealismScore { get; set; }
    public int GoalClarityScore { get; set; }
    public int PainPointsScore { get; set; }
    public int RelevanceScore { get; set; }
    public int DifferentiationScore { get; set; }
}