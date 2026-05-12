using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class ScenarioRepository(RequirementAIContext context): IScenarioRepository
{
    public async Task<Scenario> GetById(Guid id, CancellationToken ct)
    {
        return await context.Scenarios.FirstOrDefaultAsync(s => s.Id == id, ct)
            ?? throw new EntityNotFoundException<Scenario>(id);
    }

    public async Task<IList<Scenario>> GetByPersona(Guid personaId, CancellationToken ct)
    {
        return await context.Scenarios
            .Where(s => s.PersonaId == personaId)
            .ToListAsync(ct);
    }

    public async Task<Scenario> Create(Scenario scenario, CancellationToken ct)
    {
        await context.Scenarios.AddAsync(scenario, ct);
        await context.SaveChangesAsync(ct);
        
        return scenario;
    }

    public async Task<Scenario> Update(Scenario scenario, CancellationToken ct)
    {
        context.Scenarios.Update(scenario);
        await context.SaveChangesAsync(ct);
        
        return scenario;
    }

    public async Task Delete(Scenario scenario, CancellationToken ct)
    {
        context.Scenarios.Remove(scenario);
        await context.SaveChangesAsync(ct);
    }
}