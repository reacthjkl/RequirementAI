using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IQualityScoreRepository
{
    public Task<List<PersonaQualityScore>> GetPersonaQualityScores(Guid personaId, CancellationToken ct);
    public Task<List<ScenarioQualityScore>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct);
    public Task<List<UserStoryQualityScore>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct);
}