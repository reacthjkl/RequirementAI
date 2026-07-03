using RequirementAI.Business.Interfaces.QualityAnalysis;

namespace RequirementAI.Workers.Services;

public class AnalysisWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var processor = scope.ServiceProvider
                .GetRequiredService<IAnalysisJobProcessor>();

            await processor.ProcessNextJob(ct);

            await Task.Delay(5000, ct);
        }
    }
}