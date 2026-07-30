using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class EdgeCaseRepository(RequirementAIContext context): IEdgeCaseRepository
{
    public async Task<EdgeCase> GetById(Guid id, CancellationToken ct)
    {
        return await context.EdgeCases.FirstOrDefaultAsync(ec => ec.Id == id, ct)
            ?? throw new EntityNotFoundException<EdgeCase>(id);
    }

    public async Task<EdgeCase> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        return await context.EdgeCases
                   .FirstOrDefaultAsync(
                       ec => ec.Id == id &&
                             ec.UserStory.Scenario.Persona.Project.OrganizationId == organizationId,
                       ct)
               ?? throw new EntityNotFoundException<EdgeCase>(id);
    }

    public async Task<IList<EdgeCase>> GetByUserStoryId(Guid userStoryId, CancellationToken ct)
    {
        return await context.EdgeCases
            .Where(ec => ec.UserStoryId == userStoryId)
            .ToListAsync(ct);
    }

    public async Task<IList<EdgeCase>> GetByUserStoryId(Guid userStoryId, Guid organizationId, CancellationToken ct)
    {
        return await context.EdgeCases
            .Where(ec => ec.UserStoryId == userStoryId &&
                         ec.UserStory.Scenario.Persona.Project.OrganizationId == organizationId)
            .ToListAsync(ct);
    }

    public async Task<EdgeCase> Create(EdgeCase edgeCase, CancellationToken ct)
    {
        await context.EdgeCases.AddAsync(edgeCase, ct);
        await context.SaveChangesAsync(ct);

        return edgeCase;

    }

    public async Task<EdgeCase> Update(EdgeCase edgeCase, CancellationToken ct)
    {
        context.EdgeCases.Update(edgeCase);
        await context.SaveChangesAsync(ct);

        return edgeCase;    }

    public async Task Delete(EdgeCase edgeCase, CancellationToken ct)
    {
        context.Remove(edgeCase);
        await context.SaveChangesAsync(ct);
    }
}
