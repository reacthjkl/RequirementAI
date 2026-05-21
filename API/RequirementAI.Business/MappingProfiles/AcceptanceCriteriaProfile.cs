using AutoMapper;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class AcceptanceCriteriaProfile: Profile
{
    public AcceptanceCriteriaProfile()
    {
        CreateMap<AcceptanceCriteria, AcceptanceCriteriaForLLMDto>().ReverseMap();
    }
}