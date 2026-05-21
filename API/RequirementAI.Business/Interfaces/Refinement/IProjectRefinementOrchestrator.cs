using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.Refinement;

public interface IProjectRefinementOrchestrator
{
    Task Execute(ProjectRefinementJob job, CancellationToken ct);
}