using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IJobRepository<TJob> where TJob : BaseJob
{
    public Task<TJob?> AcquireNextPendingJob(CancellationToken ct);
    public Task<TJob> Get(Guid id, CancellationToken ct);
    public Task<TJob?> GetLastByProjectId(Guid projectId, CancellationToken ct);
    Task<Dictionary<Guid, JobStatus>> GetLatestStatusesByProjectIds(List<Guid> projectIds, CancellationToken ct);
    public Task<TJob> Create(TJob job, CancellationToken ct);
    public Task<TJob> Update(TJob job, CancellationToken ct);
    public Task MarkFailed(Guid jobId, string error, CancellationToken ct);
    public Task Delete(TJob job, CancellationToken ct);
}