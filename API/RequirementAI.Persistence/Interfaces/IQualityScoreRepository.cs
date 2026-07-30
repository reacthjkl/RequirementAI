using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Projections;

namespace RequirementAI.Persistence.Interfaces;

public interface IQualityScoreRepository
{
    public Task<List<PersonaQualityScore>> GetPersonaQualityScores(Guid personaId, CancellationToken ct);
    public Task<List<PersonaQualityScore>> GetPersonaQualityScores(Guid personaId, Guid organizationId, CancellationToken ct);
    public Task<List<ScenarioQualityScore>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct);
    public Task<List<ScenarioQualityScore>> GetScenarioQualityScores(Guid scenarioId, Guid organizationId, CancellationToken ct);
    public Task<List<UserStoryQualityScore>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct);
    public Task<List<UserStoryQualityScore>> GetUserStoryQualityScores(Guid userStoryId, Guid organizationId, CancellationToken ct);
    Task<PersonaQualityScore?> GetLatestPersonaQualityScore(Guid personaId, CancellationToken ct);
    Task<PersonaQualityScore?> GetLatestPersonaQualityScore(Guid personaId, Guid organizationId, CancellationToken ct);
    Task<ScenarioQualityScore?> GetLatestScenarioQualityScore(Guid scenarioId, CancellationToken ct);
    Task<ScenarioQualityScore?> GetLatestScenarioQualityScore(Guid scenarioId, Guid organizationId, CancellationToken ct);
    Task<UserStoryQualityScore?> GetLatestUserStoryQualityScore(Guid userStoryId, CancellationToken ct);
    Task<UserStoryQualityScore?> GetLatestUserStoryQualityScore(Guid userStoryId, Guid organizationId, CancellationToken ct);
    Task<List<QualityScoreTrendProjection>> GetAllQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<QualityScoreTrendProjection>> GetAllQualityScoresByProjectId(Guid projectId, Guid organizationId, CancellationToken ct);
    Task<List<PersonaQualityScore>> GetLatestPersonaQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<PersonaQualityScore>> GetLatestPersonaQualityScoresByProjectId(Guid projectId, Guid organizationId, CancellationToken ct);
    Task<List<ScenarioQualityScore>> GetLatestScenarioQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<ScenarioQualityScore>> GetLatestScenarioQualityScoresByProjectId(Guid projectId, Guid organizationId, CancellationToken ct);
    Task<List<UserStoryQualityScore>> GetLatestUserStoryQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<UserStoryQualityScore>> GetLatestUserStoryQualityScoresByProjectId(Guid projectId, Guid organizationId, CancellationToken ct);
}
