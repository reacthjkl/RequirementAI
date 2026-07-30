using RequirementAI.Business.Interfaces.Refinement;

namespace RequirementAI.Workers.Services;

public class RefinementWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<RefinementWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var processor =
                    scope.ServiceProvider.GetRequiredService<IProjectRefinementJobProcessor>();

                await processor.ProcessNextJob(ct);

                await Task.Delay(5000, ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error in refinement worker loop.");
                await Task.Delay(5000, ct);
            }
        }
    }
}
