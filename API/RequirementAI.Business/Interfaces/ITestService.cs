namespace RequirementAI.Business.Interfaces;

public interface ITestService
{
    Task TestPersonaRefinement(CancellationToken ct);
}