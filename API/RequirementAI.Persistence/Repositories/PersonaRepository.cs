using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class PersonaRepository(RequirementAIContext context): IPersonaRepository
{
    public async Task<Persona> GetById(Guid id, CancellationToken ct)
    {
        return await context.Personas.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new EntityNotFoundException<Persona>(id);
    }

    public async Task<Persona> GetWithProjectById(Guid id, CancellationToken ct)
    {
        return await context.Personas
                   .Include(e => e.Project)
                   .FirstOrDefaultAsync(p => p.Id == id, ct) 
               ?? throw new EntityNotFoundException<Persona>(id);
    }

    public async Task<IList<Persona>> GetByProject(Guid projectId, CancellationToken ct)
    {
        return await context.Personas
            .Where(p => p.ProjectId == projectId)
            .ToListAsync(ct);
    }

    public async Task<Persona> Create(Persona persona, CancellationToken ct)
    {
        await context.Personas.AddAsync(persona, ct);
        await context.SaveChangesAsync(ct);
        
        return persona;
    }

    public async Task<Persona> Update(Persona persona, CancellationToken ct)
    {
        context.Personas.Update(persona);
        await context.SaveChangesAsync(ct);

        return persona;
    }

    public async Task Delete(Persona persona, CancellationToken ct)
    {
        context.Personas.Remove(persona);
        await context.SaveChangesAsync(ct);
    }
}