namespace RequirementAI.Business.Services.Refinement;

public interface IProjectRefinementJobProcessor
{
    Task ProcessNextJob(CancellationToken ct);
}