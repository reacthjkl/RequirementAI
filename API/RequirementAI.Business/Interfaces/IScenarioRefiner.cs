using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces;

public interface IScenarioRefiner
{
    public Task<Scenario> Process(Scenario scenario, CancellationToken ct);
}