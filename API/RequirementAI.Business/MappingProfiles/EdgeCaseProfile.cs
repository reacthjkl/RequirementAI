using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class EdgeCaseProfile: Profile
{
    public EdgeCaseProfile()
    {
        CreateMap<EdgeCase, EdgeCaseForLLMDto>().ReverseMap();
        
        CreateMap<EdgeCase, EdgeCaseResponseDto>();
        CreateMap<EdgeCaseForCreationDto, EdgeCase>();
        CreateMap<EdgeCaseForUpdateDto, EdgeCase>();
    }
}