using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class QualityAnalysisJobProcessor(IQualityAnalysisJobRepository jobRepository, IQualityAnalysisOrchestrator orchestrator, IServiceScopeFactory scopeFactory): IQualityAnalysisJobProcessor
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
            await MarkFailed(job.Id, ex.Message, ct);
        }
        finally
        {
            job.FinishedAt = DateTimeOffset.UtcNow;
            await jobRepository.Update(job, ct);
        }
    }

    private async Task MarkFailed(Guid jobId, string error, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IQualityAnalysisJobRepository>();

        await repo.MarkFailed(jobId, error, ct);
    }
}