using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectRefinementJobProcessor(IProjectRefinementOrchestrator orchestrator, IProjectRefinementJobRepository jobRepository): IProjectRefinementJobProcessor
{
    public async Task ProcessNextJob(CancellationToken ct)
    {
        var job = await jobRepository.AcquireNextPendingJob(ct);
        
        if(job == null) return;
        
        try
        {
            await orchestrator.Execute(job, ct);
            job.Status = JobStatus.Completed;
        }
        catch (Exception ex)
        {
            job.Status = JobStatus.Failed;
            job.ErrorMessage = ex.Message[..Math.Min(ex.Message.Length, 1024)];
        }
        finally
        {
            job.FinishedAt = DateTimeOffset.UtcNow;
            await jobRepository.Update(job, ct);
        }
    }
}