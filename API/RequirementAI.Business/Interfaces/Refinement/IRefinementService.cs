using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.Refinement;

public interface IRefinementService
{
    public Task<Persona> RefinePersona(Persona persona, CancellationToken ct);
    public Task<Scenario> RefineScenario(Scenario scenario, CancellationToken ct);
    public Task<UserStory> RefineUserStory(UserStory userStory, CancellationToken ct);
}