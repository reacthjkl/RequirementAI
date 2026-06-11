using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class QualityAnalysisJobService(IQualityAnalysisJobRepository repo, IMapper mapper) : IQualityAnalysisJobService
{
    public async Task<QualityAnalysisJobDto> GetById(Guid jobId, CancellationToken ct)
    {
        var job = await repo.GetJobById(jobId, ct);
        
        return mapper.Map<QualityAnalysisJobDto>(job);
    }
}