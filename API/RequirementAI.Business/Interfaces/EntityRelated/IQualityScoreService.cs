using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IQualityScoreService
{
    public Task<List<PersonaQualityScoreDto>> GetPersonaQualityScores(Guid personaId, CancellationToken ct);
    public Task<List<ScenarioQualityScoreDto>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct);
    public Task<List<UserStoryQualityScoreDto>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct);
}