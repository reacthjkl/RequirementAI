namespace RequirementAI.Business.Interfaces.QualityAnalysis;

public interface IAnalysisJobProcessor
{
    Task ProcessNextJob(CancellationToken ct);
}