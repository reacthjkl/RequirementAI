using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IQualityAnalysisJobService
{
    Task<QualityAnalysisJobDto> GetById(Guid jobId, Guid organizationId, CancellationToken ct);
}
