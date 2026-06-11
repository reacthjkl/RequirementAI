using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class QualityAnalysisJobRepository(RequirementAIContext context): IQualityAnalysisJobRepository
{
    public async Task<QualityAnalysisJob> GetJobById(Guid jobId, CancellationToken ct)
    {
        return await context.QualityAnalysisJobs
            .FirstOrDefaultAsync(j => j.Id == jobId, ct)
               ?? throw new EntityNotFoundException<QualityAnalysisJob>(jobId);
        
    }

    public async Task<QualityAnalysisJob?> AcquireNextPendingJob(CancellationToken ct)
    {
        var job = await context.QualityAnalysisJobs
            .Where(x => x.Status == JobStatus.Pending)
            .OrderBy(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);
        
        if (job is null) return null;

        // try to lock job
        var affectedRows = await context.QualityAnalysisJobs
            .Where(x => x.Id == job.Id && x.Status == JobStatus.Pending)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, JobStatus.Running)
                    .SetProperty(x => x.StartedAt, DateTimeOffset.UtcNow),
                ct);
        
        if (affectedRows == 0) return null; // job has been locked by another worker
        
        return await context.QualityAnalysisJobs
            .FirstAsync(x => x.Id == job.Id, ct);
    }

    public async Task<QualityAnalysisJob?> GetLastByProjectId(Guid projectId, CancellationToken ct)
    {
        return await context.QualityAnalysisJobs
            .Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(ct);    
    }

    public async Task<QualityAnalysisJob> Create(QualityAnalysisJob job, CancellationToken ct)
    {
        await context.QualityAnalysisJobs.AddAsync(job, ct);
        await context.SaveChangesAsync(ct);
        
        return job;
        
    }

    public async Task<QualityAnalysisJob> Update(QualityAnalysisJob job, CancellationToken ct)
    {
        context.QualityAnalysisJobs.Update(job);
        await context.SaveChangesAsync(ct);

        return job;
    }

    public async Task MarkFailed(Guid jobId, string error, CancellationToken ct)
    {
        var errorMessage = error.Length > 1024
            ? error[..1024]
            : error;

        await context.QualityAnalysisJobs
            .Where(x => x.Id == jobId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, JobStatus.Failed)
                    .SetProperty(x => x.ErrorMessage, errorMessage)
                    .SetProperty(x => x.FinishedAt, DateTimeOffset.UtcNow),
                ct);
    }
}