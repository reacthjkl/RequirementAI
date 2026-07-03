using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Business.Services.Refinement;

namespace RequirementAI.Workers.Services;

public class RefinementWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var processor =
                scope.ServiceProvider.GetRequiredService<IProjectRefinementJobProcessor>();

            await processor.ProcessNextJob(ct);

            await Task.Delay(5000, ct);
        }
    }
}
