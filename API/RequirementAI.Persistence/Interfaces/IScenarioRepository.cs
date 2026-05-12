using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IScenarioRepository
{
    Task<Scenario> GetById(Guid id, CancellationToken ct);
    Task<IList<Scenario>> GetByPersona(Guid personaId, CancellationToken ct);
    Task<Scenario> Create(Scenario scenario, CancellationToken ct);
    Task<Scenario> Update(Scenario scenario,  CancellationToken ct);
    Task Delete(Scenario scenario,  CancellationToken ct);
}