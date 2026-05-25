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

        var allScores = personaScores.Select(x => x.OverallScore)
            .Concat(scenarioScores.Select(x => x.OverallScore))
            .Concat(userStoryScores.Select(x => x.OverallScore))
            .ToList();

        return new ProjectQualityOverviewDto
        {
            TotalProjectScore = allScores.Count == 0 ? 0 : Math.Round(allScores.Average(), 1),

            AveragePersonaScore = personaScores.Count == 0 ? 0 : Math.Round(personaScores.Average(x => x.OverallScore), 1),
            AverageScenarioScore = scenarioScores.Count == 0 ? 0 : Math.Round(scenarioScores.Average(x => x.OverallScore), 1),
            AverageUserStoryScore = userStoryScores.Count == 0 ? 0 : Math.Round(userStoryScores.Average(x => x.OverallScore), 1),

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
            .Select(x => new ProjectScoreTrendPointDto
            {
                Date = x.Key,
                Score = Math.Round(x.Average(s => s.OverallScore), 1),
                Label = x.Key.ToString("dd.MM")
            })
            .ToList();
    }
}