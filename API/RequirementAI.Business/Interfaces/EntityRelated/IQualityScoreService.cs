using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IQualityScoreService
{
    public Task<List<PersonaQualityScoreDto>> GetPersonaQualityScores(Guid personaId, CancellationToken ct);
    public Task<List<ScenarioQualityScoreDto>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct);
    public Task<List<UserStoryQualityScoreDto>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct);
    Task<PersonaQualityScoreDto?> GetLatestPersonaQualityScore(Guid personaId, CancellationToken ct);
    Task<ScenarioQualityScoreDto?> GetLatestScenarioQualityScore(Guid scenarioId, CancellationToken ct);
    Task<UserStoryQualityScoreDto?> GetLatestUserStoryQualityScore(Guid userStoryId, CancellationToken ct);
    Task<ProjectQualityOverviewDto> GetProjectQualityOverview(Guid projectId, CancellationToken ct);
}
