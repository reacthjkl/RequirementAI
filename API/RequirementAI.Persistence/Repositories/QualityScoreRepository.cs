using Microsoft.EntityFrameworkCore;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class QualityScoreRepository(RequirementAIContext context): IQualityScoreRepository
{
    public async Task<List<PersonaQualityScore>> GetPersonaQualityScores(Guid personaId, CancellationToken ct)
    {
        return await context.PersonaQualityScores
            .Where(x => x.PersonaId == personaId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<ScenarioQualityScore>> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct)
    {
        return await context.ScenarioQualityScores
            .Where(x => x.ScenarioId == scenarioId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<UserStoryQualityScore>> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct)
    {
        return await context.UserStoryQualityScores
            .Where(x => x.UserStoryId == userStoryId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}