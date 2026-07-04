using Microsoft.Extensions.Configuration;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectAnalysisFreshnessService(
    IProjectRepository projectRepository,
    IJobRepository<QualityAnalysisJob> analysisJobRepository,
    IConfiguration configuration) : IProjectAnalysisFreshnessService
{
    private const int DefaultAnalysisFreshnessHours = 24;
    private const int DefaultAnalysisPollingIntervalSeconds = 1;
    private const int DefaultAnalysisWaitTimeoutMinutes = 30;

    public async Task EnsureRecentAnalysis(Guid projectId, CancellationToken ct)
    {
        var latestAnalysisJob = await analysisJobRepository.GetLastCompletedByProjectId(projectId, ct);
        var latestContentUpdate = await projectRepository.GetLatestContentUpdate(projectId, ct);
        if (IsRecentAndCurrent(latestAnalysisJob, latestContentUpdate))
            return;

        var latestJob = await analysisJobRepository.GetLastByProjectId(projectId, ct);
        if (IsPendingOrRetryable(latestJob))
        {
            await WaitForCompletion(latestJob!.Id, ct);
            return;
        }

        var analysisJob = await analysisJobRepository.Create(
            new QualityAnalysisJob { ProjectId = projectId },
            ct);

        await WaitForCompletion(analysisJob.Id, ct);
    }

    private bool IsRecentAndCurrent(
        QualityAnalysisJob? analysisJob,
        DateTimeOffset latestContentUpdate)
    {
        if (analysisJob?.FinishedAt == null)
            return false;

        var freshnessHours = configuration.GetValue(
            "Refinement:AnalysisFreshnessHours",
            DefaultAnalysisFreshnessHours);
        var freshnessThreshold = DateTimeOffset.UtcNow.AddHours(-freshnessHours);

        return analysisJob.FinishedAt >= freshnessThreshold
               && analysisJob.FinishedAt >= latestContentUpdate;
    }

    private static bool IsPendingOrRetryable(QualityAnalysisJob? analysisJob)
    {
        return analysisJob?.Status is JobStatus.Pending or JobStatus.Running
               || analysisJob is { Status: JobStatus.Failed, TryCount: < 3 };
    }

    private async Task WaitForCompletion(Guid jobId, CancellationToken ct)
    {
        var timeoutMinutes = configuration.GetValue(
            "Refinement:AnalysisWaitTimeoutMinutes",
            DefaultAnalysisWaitTimeoutMinutes);
        var pollingIntervalSeconds = configuration.GetValue(
            "Refinement:AnalysisPollingIntervalSeconds",
            DefaultAnalysisPollingIntervalSeconds);

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(TimeSpan.FromMinutes(timeoutMinutes));

        try
        {
            while (true)
            {
                var job = await analysisJobRepository.Get(jobId, timeoutCts.Token);

                if (job.Status == JobStatus.Completed)
                    return;

                if (job.Status == JobStatus.Failed && job.TryCount >= 3)
                    throw new BusinessException(
                        $"Project analysis failed: {job.ErrorMessage ?? "Unknown error"}");

                await Task.Delay(
                    TimeSpan.FromSeconds(pollingIntervalSeconds),
                    timeoutCts.Token);
            }
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new BusinessException(
                $"Project analysis did not complete within {timeoutMinutes} minutes.");
        }
    }
}
