using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public abstract class JobProcessor<TJob>(IJobRepository<TJob> jobRepository)
    where TJob : BaseJob
{
    protected abstract Task Execute(TJob job, CancellationToken ct);

    public async Task ProcessNextJob(CancellationToken ct)
    {
        var job = await jobRepository.AcquireNextPendingJob(ct);

        if (job == null)
            return;

        try
        {
            await Execute(job, ct);
        }
        catch (Exception ex)
        {
            await jobRepository.MarkFailed(job.Id, ex.Message, ct);
            return;
        }

        job.Status = JobStatus.Completed;
        job.FinishedAt = DateTimeOffset.UtcNow;
        await jobRepository.Update(job, ct);
    }
}
