using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class OrganizationRepository(RequirementAIContext context): IOrganizationRepository
{
    public async Task<Organization> GetById(Guid id, CancellationToken ct)
    {
        return await context.Organizations.FirstOrDefaultAsync(o =>  o.Id == id, ct)
            ?? throw new EntityNotFoundException<Organization>(id);
    }

    public async Task<IList<Organization>> GetAll(CancellationToken ct)
    {
        return await context.Organizations.ToListAsync(ct);
    }

    public async Task<Organization> Create(Organization organization, CancellationToken ct)
    {
        await context.Organizations.AddAsync(organization, ct);
        await context.SaveChangesAsync(ct);

        return organization;
    }

    public async Task<Organization> Update(Organization organization, CancellationToken ct)
    {
        context.Organizations.Update(organization);
        await context.SaveChangesAsync(ct);
        
        return organization;
    }

    public async Task Delete(Organization organization, CancellationToken ct)
    {
        context.Organizations.Remove(organization);
        await context.SaveChangesAsync(ct);
    }
}