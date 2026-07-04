using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class AnalysisJobProcessor(
    IJobRepository<QualityAnalysisJob> jobRepository,
    IAnalysisJobOrchestrator orchestrator,
    ILLMRouteResolver routeResolver
) : JobProcessor<QualityAnalysisJob>(jobRepository, routeResolver, LLMRequestPurpose.Analysis),
    IAnalysisJobProcessor
{
    protected override Task Execute(QualityAnalysisJob job, CancellationToken ct) =>
        orchestrator.Execute(job, ct);
}
