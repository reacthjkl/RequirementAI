using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class AcceptanceCriteriaProfile: Profile
{
    public AcceptanceCriteriaProfile()
    {
        CreateMap<AcceptanceCriteria, AcceptanceCriteriaForLLMDto>().ReverseMap();
        
        CreateMap<AcceptanceCriteria, AcceptanceCriteriaResponseDto>();
        CreateMap<AcceptanceCriteriaForCreationDto, AcceptanceCriteria>();
        CreateMap<AcceptanceCriteriaForUpdateDto, AcceptanceCriteria>();
    }
}