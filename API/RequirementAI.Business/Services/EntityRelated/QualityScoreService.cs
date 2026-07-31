using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;
using RequirementAI.Persistence.Projections;

namespace RequirementAI.Business.Services.EntityRelated;

public class QualityScoreService(
    IQualityScoreRepository repository,
    IProjectRepository projectRepository,
    IMapper mapper): IQualityScoreService
{
    public async Task<List<PersonaQualityScoreDto>> GetPersonaQualityScores(
        Guid personaId,
        Guid organizationId,
        CancellationToken ct)
    {
        var scores = await repository.GetPersonaQualityScores(personaId, organizationId, ct);
        return mapper.Map<List<PersonaQualityScoreDto>>(scores);
    }

    public async Task<List<ScenarioQualityScoreDto>> GetScenarioQualityScores(
        Guid scenarioId,
        Guid organizationId,
        CancellationToken ct)
    {
        var scores = await repository.GetScenarioQualityScores(scenarioId, organizationId, ct);
        return mapper.Map<List<ScenarioQualityScoreDto>>(scores);
    }

    public async Task<List<UserStoryQualityScoreDto>> GetUserStoryQualityScores(
        Guid userStoryId,
        Guid organizationId,
        CancellationToken ct)
    {
        var scores = await repository.GetUserStoryQualityScores(userStoryId, organizationId, ct);
        return mapper.Map<List<UserStoryQualityScoreDto>>(scores);
    }

    public async Task<PersonaQualityScoreDto?> GetLatestPersonaQualityScore(
        Guid personaId,
        Guid organizationId,
        CancellationToken ct)
    {
        var score = await repository.GetLatestPersonaQualityScore(personaId, organizationId, ct);
        return mapper.Map<PersonaQualityScoreDto?>(score);
    }

    public async Task<ScenarioQualityScoreDto?> GetLatestScenarioQualityScore(
        Guid scenarioId,
        Guid organizationId,
        CancellationToken ct)
    {
        var score = await repository.GetLatestScenarioQualityScore(scenarioId, organizationId, ct);
        return mapper.Map<ScenarioQualityScoreDto?>(score);
    }

    public async Task<UserStoryQualityScoreDto?> GetLatestUserStoryQualityScore(
        Guid userStoryId,
        Guid organizationId,
        CancellationToken ct)
    {
        var score = await repository.GetLatestUserStoryQualityScore(userStoryId, organizationId, ct);
        return mapper.Map<UserStoryQualityScoreDto?>(score);
    }

    public async Task<ProjectQualityOverviewDto> GetProjectQualityOverview(
        Guid projectId,
        Guid organizationId,
        CancellationToken ct)
    {
        await projectRepository.GetById(projectId, organizationId, ct);

        var personaScores = await repository.GetLatestPersonaQualityScoresByProjectId(projectId, organizationId, ct);
        var scenarioScores = await repository.GetLatestScenarioQualityScoresByProjectId(projectId, organizationId, ct);
        var userStoryScores = await repository.GetLatestUserStoryQualityScoresByProjectId(projectId, organizationId, ct);

        var averagePersonaScore = AverageOrZero(personaScores.Select(x => x.OverallScore));
        var averageScenarioScore = AverageOrZero(scenarioScores.Select(x => x.OverallScore));
        var averageUserStoryScore = AverageOrZero(userStoryScores.Select(x => x.OverallScore));

        var totalProjectScore = CalculateTotalProjectScore(
            averagePersonaScore,
            averageScenarioScore,
            averageUserStoryScore);

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

            CriterionAverages = GetCriterionAverages(personaScores, scenarioScores, userStoryScores),

            ScoreTrend = await GetProjectScoreTrend(projectId, organizationId, ct)
        };
    }

    private static List<ProjectQualityCriterionAverageDto> GetCriterionAverages(
        List<PersonaQualityScore> personaScores,
        List<ScenarioQualityScore> scenarioScores,
        List<UserStoryQualityScore> userStoryScores)
    {
        return
        [
            CriterionAverage(
                "Persona",
                nameof(PersonaQualityScore.ClarityScore),
                personaScores.Select(x => x.ClarityScore)),
            CriterionAverage(
                "Persona",
                nameof(PersonaQualityScore.RealismScore),
                personaScores.Select(x => x.RealismScore)),
            CriterionAverage(
                "Persona",
                nameof(PersonaQualityScore.GoalClarityScore),
                personaScores.Select(x => x.GoalClarityScore)),
            CriterionAverage(
                "Persona",
                nameof(PersonaQualityScore.PainPointsScore),
                personaScores.Select(x => x.PainPointsScore)),
            CriterionAverage(
                "Persona",
                nameof(PersonaQualityScore.RelevanceScore),
                personaScores.Select(x => x.RelevanceScore)),
            CriterionAverage(
                "Persona",
                nameof(PersonaQualityScore.DifferentiationScore),
                personaScores.Select(x => x.DifferentiationScore)),

            CriterionAverage(
                "Scenario",
                nameof(ScenarioQualityScore.ClarityScore),
                scenarioScores.Select(x => x.ClarityScore)),
            CriterionAverage(
                "Scenario",
                nameof(ScenarioQualityScore.ContextScore),
                scenarioScores.Select(x => x.ContextScore)),
            CriterionAverage(
                "Scenario",
                nameof(ScenarioQualityScore.TriggerScore),
                scenarioScores.Select(x => x.TriggerScore)),
            CriterionAverage(
                "Scenario",
                nameof(ScenarioQualityScore.FlowCompletenessScore),
                scenarioScores.Select(x => x.FlowCompletenessScore)),
            CriterionAverage(
                "Scenario",
                nameof(ScenarioQualityScore.EdgeCasesScore),
                scenarioScores.Select(x => x.EdgeCasesScore)),
            CriterionAverage(
                "Scenario",
                nameof(ScenarioQualityScore.PersonaFitScore),
                scenarioScores.Select(x => x.PersonaFitScore)),

            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.ClarityScore),
                userStoryScores.Select(x => x.ClarityScore)),
            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.CompletenessScore),
                userStoryScores.Select(x => x.CompletenessScore)),
            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.TestabilityScore),
                userStoryScores.Select(x => x.TestabilityScore)),
            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.AcceptanceCriteriaScore),
                userStoryScores.Select(x => x.AcceptanceCriteriaScore)),
            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.ScopeScore),
                userStoryScores.Select(x => x.ScopeScore)),
            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.BusinessValueScore),
                userStoryScores.Select(x => x.BusinessValueScore)),
            CriterionAverage(
                "UserStory",
                nameof(UserStoryQualityScore.AmbiguityScore),
                userStoryScores.Select(x => x.AmbiguityScore))
        ];
    }

    private static ProjectQualityCriterionAverageDto CriterionAverage(
        string artifactType,
        string criterionName,
        IEnumerable<int> scores)
    {
        return new ProjectQualityCriterionAverageDto
        {
            ArtifactType = artifactType,
            CriterionName = criterionName,
            AverageScore = AverageOrZero(scores)
        };
    }

    private async Task<List<ProjectScoreTrendPointDto>> GetProjectScoreTrend(
        Guid projectId,
        Guid organizationId,
        CancellationToken ct)
    {
        var scores = await repository.GetAllQualityScoresByProjectId(projectId, organizationId, ct);

        return scores
            .GroupBy(x => x.CreatedAt)
            .OrderBy(x => x.Key)
            .Select(analysis =>
            {
                var averagePersonaScore = AverageOrZero(
                    analysis
                        .Where(x => x.Type == QualityScoreTrendType.Persona)
                        .Select(x => x.OverallScore));

                var averageScenarioScore = AverageOrZero(
                    analysis
                        .Where(x => x.Type == QualityScoreTrendType.Scenario)
                        .Select(x => x.OverallScore));

                var averageUserStoryScore = AverageOrZero(
                    analysis
                        .Where(x => x.Type == QualityScoreTrendType.UserStory)
                        .Select(x => x.OverallScore));

                var totalProjectScore = CalculateTotalProjectScore(
                    averagePersonaScore,
                    averageScenarioScore,
                    averageUserStoryScore);

                return new ProjectScoreTrendPointDto
                {
                    Date = analysis.Key,
                    Score = Convert.ToDouble(totalProjectScore),
                    Label = analysis.Key.ToString("dd.MM HH:mm")
                };
            })
            .ToList();
    }

    private static decimal CalculateTotalProjectScore(
        decimal averagePersonaScore,
        decimal averageScenarioScore,
        decimal averageUserStoryScore)
    {
        return AverageOrZero(new[]
        {
            averagePersonaScore,
            averageScenarioScore,
            averageUserStoryScore
        }.Where(x => x > 0));
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
