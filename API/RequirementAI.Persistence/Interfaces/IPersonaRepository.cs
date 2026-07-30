using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IPersonaRepository
{
    Task<Persona> GetById(Guid id, CancellationToken ct);
    Task<Persona> GetById(Guid id, Guid organizationId, CancellationToken ct);
    Task<Persona> GetWithProjectById(Guid guid, CancellationToken ct);
    Task<IList<Persona>> GetByProject(Guid projectId, CancellationToken ct);
    Task<IList<Persona>> GetByProject(Guid projectId, Guid organizationId, CancellationToken ct);
    Task<Persona> Create(Persona persona, CancellationToken ct);
    Task<Persona> Update(Persona persona,  CancellationToken ct);
    Task Delete(Persona persona, CancellationToken ct);
}
