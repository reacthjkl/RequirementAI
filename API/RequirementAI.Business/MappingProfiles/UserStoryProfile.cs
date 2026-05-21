using AutoMapper;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class UserStoryProfile: Profile
{
    public UserStoryProfile()
    {
        CreateMap<UserStory, UserStoryForLLMDto>().ReverseMap();
    }
}