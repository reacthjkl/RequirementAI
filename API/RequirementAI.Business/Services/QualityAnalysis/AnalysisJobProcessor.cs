using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class AnalysisJobProcessor(
    IJobRepository<QualityAnalysisJob> jobRepository,
    IAnalysisJobOrchestrator orchestrator
) : JobProcessor<QualityAnalysisJob>(jobRepository), IAnalysisJobProcessor
{
    protected override Task Execute(QualityAnalysisJob job, CancellationToken ct) =>
        orchestrator.Execute(job, ct);
}
