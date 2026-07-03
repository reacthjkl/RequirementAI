using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class QualityAnalysisJobService(IJobRepository<QualityAnalysisJob> repo, IMapper mapper)
    : IQualityAnalysisJobService
{
    public async Task<QualityAnalysisJobDto> GetById(Guid jobId, CancellationToken ct)
    {
        var job = await repo.Get(jobId, ct);

        return mapper.Map<QualityAnalysisJobDto>(job);
    }
}