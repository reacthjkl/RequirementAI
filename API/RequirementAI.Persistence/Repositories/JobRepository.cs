using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class JobRepository<TJob>(RequirementAIContext context) : IJobRepository<TJob> where TJob : BaseJob
{
    public async Task<TJob?> AcquireNextPendingJob(CancellationToken ct)
    {
        Expression<Func<TJob, bool>> predicate = x =>
            x.Status == JobStatus.Pending ||
            (x.Status == JobStatus.Failed && x.TryCount < 3);

        var jobId = await context
            .Set<TJob>()
            .Where(predicate)
            .OrderBy(x => x.CreatedAt)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync(ct);

        if (jobId is null)
            return null;

        // try to lock job
        var affectedRows = await context
            .Set<TJob>()
            .Where(predicate)
            .Where(x => x.Id == jobId.Value)
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(x => x.Status, JobStatus.Running)
                        .SetProperty(x => x.StartedAt, DateTimeOffset.UtcNow)
                        .SetProperty(x => x.TryCount, x => x.TryCount + 1),
                ct
            );

        if (affectedRows == 0)
            return null; // job has been locked by another worker

        return await context.Set<TJob>().FirstAsync(x => x.Id == jobId.Value, ct);
    }

    public async Task<Dictionary<Guid, JobStatus>> GetLatestStatusesByProjectIds(
        List<Guid> projectIds,
        CancellationToken ct
    )
    {
        return await context
            .Set<TJob>().Where(j => projectIds.Contains(j.ProjectId))
            .GroupBy(j => j.ProjectId)
            .Select(g =>
                g.OrderByDescending(j => j.CreatedAt)
                    .Select(j => new { j.ProjectId, j.Status })
                    .First()
            )
            .ToDictionaryAsync(x => x.ProjectId, x => x.Status, ct);
    }

    public async Task MarkFailed(Guid jobId, string error, CancellationToken ct)
    {
        var errorMessage = error.Length > 1024 ? error[..1024] : error;

        await context
            .Set<TJob>().Where(x => x.Id == jobId)
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(x => x.Status, JobStatus.Failed)
                        .SetProperty(x => x.ErrorMessage, errorMessage)
                        .SetProperty(x => x.FinishedAt, DateTimeOffset.UtcNow),
                ct
            );
    }

    public async Task<TJob> Get(Guid id, CancellationToken ct)
    {
        return await context.Set<TJob>().FirstOrDefaultAsync(x => x.Id == id, ct)
               ?? throw new EntityNotFoundException<ProjectRefinementJob>(id);
    }

    public async Task<TJob?> GetLastByProjectId(
        Guid projectId,
        CancellationToken ct
    )
    {
        return await context
            .Set<TJob>().Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<TJob> Create(TJob job, CancellationToken ct)
    {
        await context.Set<TJob>().AddAsync(job, ct);
        await context.SaveChangesAsync(ct);

        return job;
    }

    public async Task<TJob> Update(TJob job, CancellationToken ct)
    {
        context.Set<TJob>().Update(job);
        await context.SaveChangesAsync(ct);

        return job;
    }

    public async Task Delete(TJob job, CancellationToken ct)
    {
        context.Set<TJob>().Remove(job);
        await context.SaveChangesAsync(ct);
    }
}