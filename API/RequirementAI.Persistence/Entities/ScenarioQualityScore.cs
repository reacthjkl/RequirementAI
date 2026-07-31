namespace RequirementAI.Persistence.Entities;

public class ScenarioQualityScore: QualityScoreBase
{
    public Guid ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    public decimal ClarityScore { get; set; }
    public decimal ContextScore { get; set; }
    public decimal TriggerScore { get; set; }
    public decimal FlowCompletenessScore { get; set; }
    public decimal EdgeCasesScore { get; set; }
    public decimal PersonaFitScore { get; set; }
}
