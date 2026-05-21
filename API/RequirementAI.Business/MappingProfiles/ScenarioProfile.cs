using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class ScenarioProfile: Profile
{
    public ScenarioProfile()
    {
        CreateMap<Scenario, ScenarioForLLMDto>().ReverseMap();
        
        CreateMap<Scenario, ScenarioResponseDto>();
        CreateMap<ScenarioForCreationDto, Scenario>();
        CreateMap<ScenarioForUpdateDto, Scenario>();
    }
}