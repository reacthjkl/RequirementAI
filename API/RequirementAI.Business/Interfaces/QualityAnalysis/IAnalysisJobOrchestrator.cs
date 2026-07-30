using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.QualityAnalysis;

public interface IAnalysisJobOrchestrator
{
    Task Execute(QualityAnalysisJob job, CancellationToken ct);
}