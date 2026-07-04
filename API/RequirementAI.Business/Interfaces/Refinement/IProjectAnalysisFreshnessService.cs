namespace RequirementAI.Business.Interfaces.Refinement;

public interface IProjectAnalysisFreshnessService
{
    Task EnsureRecentAnalysis(Guid projectId, CancellationToken ct);
}
