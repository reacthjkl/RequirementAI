using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.Refinement;

public interface IRefinementService
{
    public Task<Persona> RefinePersona(Persona persona, string? jobCustomInstructions, CancellationToken ct);
    public Task<Scenario> RefineScenario(Scenario scenario, string? jobCustomInstructions, CancellationToken ct);
    public Task<UserStory> RefineUserStory(UserStory userStory, string? jobCustomInstructions, CancellationToken ct);
}