namespace RequirementAI.Contract.Dto;

public class ProjectQualityOverviewDto
{
    public decimal TotalProjectScore { get; set; }

    public decimal AveragePersonaScore { get; set; }
    public decimal AverageScenarioScore { get; set; }
    public decimal AverageUserStoryScore { get; set; }

    public LowestScoreItemDto? LowestPersona { get; set; }
    public LowestScoreItemDto? LowestScenario { get; set; }
    public LowestScoreItemDto? LowestUserStory { get; set; }

    public List<ProjectQualityCriterionAverageDto> CriterionAverages { get; set; } = [];

    public List<ProjectScoreTrendPointDto> ScoreTrend { get; set; } = [];
}
