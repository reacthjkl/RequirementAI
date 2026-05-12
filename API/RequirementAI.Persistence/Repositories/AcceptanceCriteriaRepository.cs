using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class AcceptanceCriteriaRepository(RequirementAIContext context): IAcceptanceCriteriaRepository
{
    public async Task<AcceptanceCriteria> GetById(Guid id, CancellationToken ct)
    {
        return await context.AcceptanceCriteria.FirstOrDefaultAsync(ac => ac.Id == id, ct)
            ?? throw new EntityNotFoundException<AcceptanceCriteria>(id);
    }

    public async Task<IList<AcceptanceCriteria>> GetByUserStoryId(Guid userStoryId, CancellationToken ct)
    {
        return await context.AcceptanceCriteria
            .Where(ac => ac.UserStoryId == userStoryId)
            .ToListAsync(ct);
    }

    public async Task<AcceptanceCriteria> Create(AcceptanceCriteria acceptanceCriteria, CancellationToken ct)
    {
        context.AcceptanceCriteria.Add(acceptanceCriteria);
        await context.SaveChangesAsync(ct);
        
        return  acceptanceCriteria;
    }

    public async Task<AcceptanceCriteria> Update(AcceptanceCriteria acceptanceCriteria, CancellationToken ct)
    {
        context.AcceptanceCriteria.Update(acceptanceCriteria);
        await context.SaveChangesAsync(ct);
        
        return acceptanceCriteria;
    }

    public async Task Delete(AcceptanceCriteria acceptanceCriteria, CancellationToken ct)
    {
        context.AcceptanceCriteria.Remove(acceptanceCriteria);
        await context.SaveChangesAsync(ct);
    }
}