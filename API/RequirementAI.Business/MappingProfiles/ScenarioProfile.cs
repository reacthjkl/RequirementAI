using AutoMapper;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class ScenarioProfile: Profile
{
    public ScenarioProfile()
    {
        CreateMap<Scenario, ScenarioForLLMDto>().ReverseMap();
    }
}