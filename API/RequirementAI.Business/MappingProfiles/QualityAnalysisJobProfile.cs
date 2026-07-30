using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class QualityAnalysisJobProfile:Profile
{
    public QualityAnalysisJobProfile()
    {
        CreateMap<QualityAnalysisJob, QualityAnalysisJobDto>().ReverseMap();
    }
}