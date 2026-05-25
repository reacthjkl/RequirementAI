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
}