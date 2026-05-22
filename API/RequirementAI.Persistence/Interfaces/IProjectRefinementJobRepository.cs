using System.Linq.Expressions;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IProjectRefinementJobRepository
{
    public Task<ProjectRefinementJob> Get(Guid id, CancellationToken ct);
    Task<Dictionary<Guid, JobStatus>> GetLatestStatusesByProjectIds(List<Guid> projectIds, CancellationToken ct);
    public Task<ProjectRefinementJob?> AcquireNextPendingJob(CancellationToken ct);
    public Task<ProjectRefinementJob> Create(ProjectRefinementJob job, CancellationToken ct);
    public Task<ProjectRefinementJob> Update(ProjectRefinementJob job, CancellationToken ct);
    public Task Delete(ProjectRefinementJob job, CancellationToken ct);
}