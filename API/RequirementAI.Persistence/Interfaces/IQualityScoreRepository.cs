using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Projections;

namespace RequirementAI.Persistence.Interfaces;

public interface IQualityScoreRepository
{
    public Task<List<PersonaQualityScore>> GetPersonaQualityScores(Guid personaId, CancellationToken ct);
    public Task<List<ScenarioQualityScore>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct);
    public Task<List<UserStoryQualityScore>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct);
    Task<List<QualityScoreTrendProjection>> GetAllQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<PersonaQualityScore>> GetLatestPersonaQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<ScenarioQualityScore>> GetLatestScenarioQualityScoresByProjectId(Guid projectId, CancellationToken ct);
    Task<List<UserStoryQualityScore>> GetLatestUserStoryQualityScoresByProjectId(Guid projectId, CancellationToken ct);
}