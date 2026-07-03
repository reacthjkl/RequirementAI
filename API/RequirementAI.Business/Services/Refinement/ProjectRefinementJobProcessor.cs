using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectRefinementJobProcessor(
    IProjectRefinementOrchestrator orchestrator,
    IJobRepository<ProjectRefinementJob> jobRepository
) : JobProcessor<ProjectRefinementJob>(jobRepository), IProjectRefinementJobProcessor
{
    protected override Task Execute(ProjectRefinementJob job, CancellationToken ct) =>
        orchestrator.Execute(job, ct);
}
