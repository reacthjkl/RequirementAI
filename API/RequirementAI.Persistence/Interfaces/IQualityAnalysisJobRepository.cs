using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IQualityAnalysisJobRepository
{
    Task<QualityAnalysisJob> GetJobById(Guid jobId, CancellationToken ct);
    Task<QualityAnalysisJob?> AcquireNextPendingJob(CancellationToken ct);
    Task<QualityAnalysisJob?> GetLastByProjectId(Guid projectId, CancellationToken ct);
    Task<QualityAnalysisJob> Create(QualityAnalysisJob projectRefinementJob, CancellationToken ct);
    Task<QualityAnalysisJob> Update(QualityAnalysisJob job, CancellationToken ct);
    Task MarkFailed(Guid jobId, string error, CancellationToken ct);
}