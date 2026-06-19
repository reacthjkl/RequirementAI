using Microsoft.EntityFrameworkCore;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;
using RequirementAI.Persistence.Projections;

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

    public async Task<PersonaQualityScore?> GetLatestPersonaQualityScore(Guid personaId, CancellationToken ct)
    {
        return await context.PersonaQualityScores
            .Where(x => x.PersonaId == personaId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ScenarioQualityScore?> GetLatestScenarioQualityScore(Guid scenarioId, CancellationToken ct)
    {
        return await context.ScenarioQualityScores
            .Where(x => x.ScenarioId == scenarioId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<UserStoryQualityScore?> GetLatestUserStoryQualityScore(Guid userStoryId, CancellationToken ct)
    {
        return await context.UserStoryQualityScores
            .Where(x => x.UserStoryId == userStoryId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<QualityScoreTrendProjection>> GetAllQualityScoresByProjectId(Guid projectId, CancellationToken ct)
    {
        var personaScores = context.PersonaQualityScores
            .Where(x => x.Persona.ProjectId == projectId)
            .Select(x => new QualityScoreTrendProjection
            {
                OverallScore = x.OverallScore,
                CreatedAt = x.CreatedAt,
                Type = QualityScoreTrendType.Persona
            });

        var scenarioScores = context.ScenarioQualityScores
            .Where(x => x.Scenario.Persona.ProjectId == projectId)
            .Select(x => new QualityScoreTrendProjection
            {
                OverallScore = x.OverallScore,
                CreatedAt = x.CreatedAt,
                Type = QualityScoreTrendType.Scenario
            });

        var userStoryScores = context.UserStoryQualityScores
            .Where(x => x.UserStory.Scenario.Persona.ProjectId == projectId)
            .Select(x => new QualityScoreTrendProjection
            {
                OverallScore = x.OverallScore,
                CreatedAt = x.CreatedAt,
                Type = QualityScoreTrendType.UserStory
            });

        return await personaScores
            .Concat(scenarioScores)
            .Concat(userStoryScores)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<PersonaQualityScore>> GetLatestPersonaQualityScoresByProjectId(Guid projectId, CancellationToken ct)
    {
        return await context.PersonaQualityScores
            .Include(x => x.Persona)
            .Where(x => x.Persona.ProjectId == projectId)
            .GroupBy(x => x.PersonaId)
            .Select(g => g
                .OrderByDescending(x => x.CreatedAt)
                .First())
            .ToListAsync(ct);
    }

    public async Task<List<ScenarioQualityScore>> GetLatestScenarioQualityScoresByProjectId(Guid projectId, CancellationToken ct)
    {
        return await context.ScenarioQualityScores
            .Include(x => x.Scenario)
            .ThenInclude(x => x.Persona)
            .Where(x => x.Scenario.Persona.ProjectId == projectId)
            .GroupBy(x => x.ScenarioId)
            .Select(g => g.OrderByDescending(x => x.CreatedAt).First())
            .ToListAsync(ct);
    }

    public async Task<List<UserStoryQualityScore>> GetLatestUserStoryQualityScoresByProjectId(Guid projectId, CancellationToken ct)
    {
        return await context.UserStoryQualityScores
            .Include(x => x.UserStory)
            .ThenInclude(x => x.Scenario)
            .ThenInclude(x => x.Persona)
            .Where(x => x.UserStory.Scenario.Persona.ProjectId == projectId)
            .GroupBy(x => x.UserStoryId)
            .Select(g => g.OrderByDescending(x => x.CreatedAt).First())
            .ToListAsync(ct);
    }
}
