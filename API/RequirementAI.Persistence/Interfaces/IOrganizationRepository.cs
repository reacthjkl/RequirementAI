using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization> GetById(Guid id, CancellationToken ct);
    Task<IList<Organization>> GetAll(CancellationToken ct);
    Task<Organization> Create(Organization organization, CancellationToken ct);
    Task<Organization> Update(Organization organization, CancellationToken ct);
    Task Delete(Organization organization, CancellationToken ct);
}