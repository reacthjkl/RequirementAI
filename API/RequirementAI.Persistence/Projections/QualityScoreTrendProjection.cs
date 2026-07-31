namespace RequirementAI.Persistence.Projections;


public enum QualityScoreTrendType
{
    Persona,
    Scenario,
    UserStory
}

public class QualityScoreTrendProjection
{
    public decimal OverallScore { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public QualityScoreTrendType Type { get; init; }

}
