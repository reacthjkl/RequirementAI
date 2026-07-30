namespace RequirementAI.Persistence.Entities;

public class ScenarioQualityScore: QualityScoreBase
{
    public Guid ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    public int ClarityScore { get; set; }
    public int ContextScore { get; set; }
    public int TriggerScore { get; set; }
    public int FlowCompletenessScore { get; set; }
    public int EdgeCasesScore { get; set; }
    public int PersonaFitScore { get; set; }
}