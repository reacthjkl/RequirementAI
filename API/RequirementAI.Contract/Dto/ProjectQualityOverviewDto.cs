namespace RequirementAI.Contract.Dto;

public class ProjectQualityOverviewDto
{
    public double TotalProjectScore { get; set; }

    public double AveragePersonaScore { get; set; }
    public double AverageScenarioScore { get; set; }
    public double AverageUserStoryScore { get; set; }

    public LowestScoreItemDto? LowestPersona { get; set; }
    public LowestScoreItemDto? LowestScenario { get; set; }
    public LowestScoreItemDto? LowestUserStory { get; set; }

    public List<ProjectScoreTrendPointDto> ScoreTrend { get; set; } = [];
}