using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class ProjectRefinementJobRepository(RequirementAIContext context): IProjectRefinementJobRepository
{
    public async Task<ProjectRefinementJob> Get(Guid id, CancellationToken ct)
    {
        return await context.ProjectRefinementJobs
            .FirstOrDefaultAsync(x => x.Id == id, ct) 
               ?? throw new EntityNotFoundException<ProjectRefinementJob>(id);
    }

    public async Task<ProjectRefinementJob?> GetLastByProjectId(Guid projectId, CancellationToken ct)
    {
        return await context.ProjectRefinementJobs
            .Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Dictionary<Guid, JobStatus>> GetLatestStatusesByProjectIds(List<Guid> projectIds, CancellationToken ct)
    {
        return await context.ProjectRefinementJobs
            .Where(j => projectIds.Contains(j.ProjectId))
            .GroupBy(j => j.ProjectId)
            .Select(g => g
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new
                {
                    j.ProjectId,
                    j.Status
                })
                .First())
            .ToDictionaryAsync(x => x.ProjectId, x => x.Status, ct);
        
    }

    public async Task<ProjectRefinementJob?> AcquireNextPendingJob(CancellationToken ct)
    {
        var job = await context.ProjectRefinementJobs
            .Where(x => x.Status == JobStatus.Pending)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
        
        if (job is null) return null;

        // try to lock job
        var affectedRows = await context.ProjectRefinementJobs
            .Where(x => x.Id == job.Id && x.Status == JobStatus.Pending)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, JobStatus.Running)
                    .SetProperty(x => x.StartedAt, DateTimeOffset.UtcNow),
                ct);
        
        if (affectedRows == 0) return null; // job has been locked by another worker
        
        return await context.ProjectRefinementJobs
            .FirstAsync(x => x.Id == job.Id, ct);
    }

    public async Task<ProjectRefinementJob> Create(ProjectRefinementJob job, CancellationToken ct)
    {
        await context.ProjectRefinementJobs.AddAsync(job, ct);
        await context.SaveChangesAsync(ct);
        
        return job;
    }

    public async Task<ProjectRefinementJob> Update(ProjectRefinementJob job, CancellationToken ct)
    {
        context.ProjectRefinementJobs.Update(job);
        await context.SaveChangesAsync(ct);
        
        return job;
    }

    public async Task MarkFailed(Guid jobId, string error, CancellationToken ct)
    {
        var errorMessage = error.Length > 1024
            ? error[..1024]
            : error;

        await context.ProjectRefinementJobs
            .Where(x => x.Id == jobId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, JobStatus.Failed)
                    .SetProperty(x => x.ErrorMessage, errorMessage)
                    .SetProperty(x => x.FinishedAt, DateTimeOffset.UtcNow),
                ct);
    }

    public async Task Delete(ProjectRefinementJob job, CancellationToken ct)
    {
        context.ProjectRefinementJobs.Remove(job);
        await context.SaveChangesAsync(ct);
    }
}