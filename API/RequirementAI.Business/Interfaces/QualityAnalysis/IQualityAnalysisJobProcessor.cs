namespace RequirementAI.Business.Interfaces.QualityAnalysis;

public interface IQualityAnalysisJobProcessor
{
    Task ProcessNextJob(CancellationToken ct);
}