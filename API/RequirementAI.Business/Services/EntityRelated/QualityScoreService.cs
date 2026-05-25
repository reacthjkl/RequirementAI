using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class QualityScoreService(IQualityScoreRepository repository, IMapper mapper): IQualityScoreService
{
    public async Task<List<PersonaQualityScoreDto>> GetPersonaQualityScores(Guid personaId, CancellationToken ct)
    {
        var scores = await repository.GetPersonaQualityScores(personaId, ct);
        return mapper.Map<List<PersonaQualityScoreDto>>(scores);
    }

    public async Task<List<ScenarioQualityScoreDto>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct)
    {
        var scores = await repository.GetScenarioQualityScores(scenarioId, ct);
        return mapper.Map<List<ScenarioQualityScoreDto>>(scores);
    }

    public async Task<List<UserStoryQualityScoreDto>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct)
    {
        var scores = await repository.GetUserStoryQualityScores(userStoryId, ct);
        return mapper.Map<List<UserStoryQualityScoreDto>>(scores);
    }

    public async Task<ProjectQualityOverviewDto> GetProjectQualityOverview(Guid projectId, CancellationToken ct)
{
    var personaScores = await repository.GetLatestPersonaQualityScoresByProjectId(projectId, ct);
    var scenarioScores = await repository.GetLatestScenarioQualityScoresByProjectId(projectId, ct);
    var userStoryScores = await repository.GetLatestUserStoryQualityScoresByProjectId(projectId, ct);

    var averagePersonaScore = AverageOrZero(personaScores.Select(x => x.OverallScore));
    var averageScenarioScore = AverageOrZero(scenarioScores.Select(x => x.OverallScore));
    var averageUserStoryScore = AverageOrZero(userStoryScores.Select(x => x.OverallScore));

    var totalProjectScore = AverageOrZero(new[]
    {
        averagePersonaScore,
        averageScenarioScore,
        averageUserStoryScore
    }.Where(x => x > 0));

    return new ProjectQualityOverviewDto
    {
        TotalProjectScore = totalProjectScore,

        AveragePersonaScore = averagePersonaScore,
        AverageScenarioScore = averageScenarioScore,
        AverageUserStoryScore = averageUserStoryScore,

        LowestPersona = personaScores
            .OrderBy(x => x.OverallScore)
            .Select(x => new LowestScoreItemDto
            {
                ItemId = x.PersonaId,
                ItemType = "Persona",
                Title = x.Persona.Name,
                Score = x.OverallScore,
                EvaluatedAt = x.CreatedAt
            })
            .FirstOrDefault(),

        LowestScenario = scenarioScores
            .OrderBy(x => x.OverallScore)
            .Select(x => new LowestScoreItemDto
            {
                ItemId = x.ScenarioId,
                ItemType = "Scenario",
                Title = x.Scenario.Title,
                Score = x.OverallScore,
                EvaluatedAt = x.CreatedAt
            })
            .FirstOrDefault(),

        LowestUserStory = userStoryScores
            .OrderBy(x => x.OverallScore)
            .Select(x => new LowestScoreItemDto
            {
                ItemId = x.UserStoryId,
                ItemType = "UserStory",
                Title = x.UserStory.Title,
                Score = x.OverallScore,
                EvaluatedAt = x.CreatedAt
            })
            .FirstOrDefault(),

        ScoreTrend = await GetProjectScoreTrend(projectId, ct)
    };
}
    private async Task<List<ProjectScoreTrendPointDto>> GetProjectScoreTrend(Guid projectId, CancellationToken ct)
    {
        var scores = await repository.GetAllQualityScoresByProjectId(projectId, ct);

        return scores
            .GroupBy(x => x.CreatedAt.Date)
            .OrderBy(x => x.Key)
            .Select(day =>
            {
                var projectScore = AverageOrZero(day.Select(x => x.OverallScore));

                return new ProjectScoreTrendPointDto
                {
                    Date = day.Key,
                    Score = Convert.ToDouble(projectScore),
                    Label = day.Key.ToString("dd.MM")
                };
            })
            .ToList();
    }

    private static decimal AverageOrZero(IEnumerable<int> scores)
    {
        var list = scores.ToList();

        return list.Count == 0
            ? 0
            : Math.Round((decimal)list.Average(), 1);
    }

    private static decimal AverageOrZero(IEnumerable<decimal> scores)
    {
        var list = scores.ToList();

        return list.Count == 0
            ? 0
            : Math.Round(list.Average(), 1);
    }
}