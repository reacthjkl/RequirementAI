namespace RequirementAI.Contract.Dto;

public class PersonaQualityScoreDto: QualityScoreBaseDto
{
    public Guid PersonaId { get; set; }

    public int ClarityScore { get; set; }
    public int RealismScore { get; set; }
    public int GoalClarityScore { get; set; }
    public int PainPointsScore { get; set; }
    public int RelevanceScore { get; set; }
    public int DifferentiationScore { get; set; }
}