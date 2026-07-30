namespace RequirementAI.Business.Interfaces.Refinement;

public interface IProjectRefinementJobProcessor
{
    Task ProcessNextJob(CancellationToken ct);
}