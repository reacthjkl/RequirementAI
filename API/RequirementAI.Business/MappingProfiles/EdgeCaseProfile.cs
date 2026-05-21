using AutoMapper;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class EdgeCaseProfile: Profile
{
    public EdgeCaseProfile()
    {
        CreateMap<EdgeCase, EdgeCaseForLLMDto>().ReverseMap();
    }
}