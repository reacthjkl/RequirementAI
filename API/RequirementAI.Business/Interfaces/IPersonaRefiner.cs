using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces;

public interface IPersonaRefiner
{
    public Task<Persona> Process(Persona persona, CancellationToken ct);
}