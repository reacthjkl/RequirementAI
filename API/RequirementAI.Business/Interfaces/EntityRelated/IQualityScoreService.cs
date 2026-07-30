using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IQualityScoreService
{
    public Task<List<PersonaQualityScoreDto>> GetPersonaQualityScores(Guid personaId, Guid organizationId, CancellationToken ct);
    public Task<List<ScenarioQualityScoreDto>> GetScenarioQualityScores(Guid scenarioId, Guid organizationId, CancellationToken ct);
    public Task<List<UserStoryQualityScoreDto>> GetUserStoryQualityScores(Guid userStoryId, Guid organizationId, CancellationToken ct);
    Task<PersonaQualityScoreDto?> GetLatestPersonaQualityScore(Guid personaId, Guid organizationId, CancellationToken ct);
    Task<ScenarioQualityScoreDto?> GetLatestScenarioQualityScore(Guid scenarioId, Guid organizationId, CancellationToken ct);
    Task<UserStoryQualityScoreDto?> GetLatestUserStoryQualityScore(Guid userStoryId, Guid organizationId, CancellationToken ct);
    Task<ProjectQualityOverviewDto> GetProjectQualityOverview(Guid projectId, Guid organizationId, CancellationToken ct);
}
