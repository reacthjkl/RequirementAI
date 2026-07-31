namespace RequirementAI.Contract.Dto;

public class ScenarioQualityScoreDto: QualityScoreBaseDto
{
    public Guid ScenarioId { get; set; }

    public decimal ClarityScore { get; set; }
    public decimal ContextScore { get; set; }
    public decimal TriggerScore { get; set; }
    public decimal FlowCompletenessScore { get; set; }
    public decimal EdgeCasesScore { get; set; }
    public decimal PersonaFitScore { get; set; }
}
