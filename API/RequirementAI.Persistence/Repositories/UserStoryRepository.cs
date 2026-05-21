using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class UserStoryRepository(RequirementAIContext context): IUserStoryRepository
{
    public async Task<UserStory> GetById(Guid id, CancellationToken ct)
    {
        return await context.UserStories.FirstOrDefaultAsync(us => us.Id == id, ct)
            ?? throw new EntityNotFoundException<UserStory>(id); 
    }

    public async Task<IList<UserStory>> GetByScenario(Guid scenarioId, CancellationToken ct)
    {
        return await context.UserStories
            .Where(us => us.ScenarioId == scenarioId)
            .ToListAsync(ct);
    }

    public async Task<IList<UserStory>> GetByPersona(Guid personaId, CancellationToken ct)
    {
        return await context.UserStories
            .Where(us => us.Scenario.PersonaId == personaId)
            .ToListAsync(ct);
    }

    public async Task<IList<UserStory>> GetByProject(Guid projectId, CancellationToken ct)
    {
        return await context.UserStories
            .Where(us => us.Scenario.Persona.ProjectId == projectId)
            .ToListAsync(ct);
    }

    public async Task<UserStory> Create(UserStory userStory, CancellationToken ct)
    {
        await context.UserStories.AddAsync(userStory, ct);
        await context.SaveChangesAsync(ct);

        return userStory;
    }

    public async Task<UserStory> Update(UserStory userStory, CancellationToken ct)
    {
        context.UserStories.Update(userStory);
        await context.SaveChangesAsync(ct);
    
        return userStory;
    }

    public async Task Delete(UserStory userStory, CancellationToken ct)
    {
        context.UserStories.Remove(userStory);
        await context.SaveChangesAsync(ct);
    }
}