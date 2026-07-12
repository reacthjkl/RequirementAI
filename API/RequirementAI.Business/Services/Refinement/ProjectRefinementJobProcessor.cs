using Microsoft.Extensions.Logging;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectRefinementJobProcessor(
    IProjectRefinementOrchestrator orchestrator,
    IJobRepository<ProjectRefinementJob> jobRepository,
    ILLMRouteResolver routeResolver,
    ILogger<ProjectRefinementJobProcessor> logger
) : JobProcessor<ProjectRefinementJob>(jobRepository, routeResolver, LLMRequestPurpose.Refinement, logger),
    IProjectRefinementJobProcessor
{
    protected override Task Execute(ProjectRefinementJob job, CancellationToken ct)
    {
        return orchestrator.Execute(job, ct);
    }
}