using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IQualityAnalysisJobRepository
{
    Task<QualityAnalysisJob?> AcquireNextPendingJob(CancellationToken ct);
    Task<QualityAnalysisJob> Update(QualityAnalysisJob job, CancellationToken ct);
    Task MarkFailed(Guid jobId, string error, CancellationToken ct);
}