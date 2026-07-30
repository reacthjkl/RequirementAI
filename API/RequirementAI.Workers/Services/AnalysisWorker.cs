using RequirementAI.Business.Interfaces.QualityAnalysis;

namespace RequirementAI.Workers.Services;

public class AnalysisWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<AnalysisWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var processor = scope.ServiceProvider
                    .GetRequiredService<IAnalysisJobProcessor>();

                await processor.ProcessNextJob(ct);

                await Task.Delay(5000, ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error in analysis worker loop.");
                await Task.Delay(5000, ct);
            }
        }
    }
}
