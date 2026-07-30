using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.QualityAnalysis;

public interface IQualityAnalysisOrchestrator
{
    Task Execute(QualityAnalysisJob job, CancellationToken ct);
}