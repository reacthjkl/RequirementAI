using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IQualityAnalysisJobService
{
    Task<QualityAnalysisJobDto> GetById(Guid jobId, CancellationToken ct);
}