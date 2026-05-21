using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class UserStoryProfile: Profile
{
    public UserStoryProfile()
    {
        CreateMap<UserStory, UserStoryForLLMDto>()
            .ForMember(s => s.AcceptanceCriteria, o => o.Ignore())
            .ForMember(s => s.EdgeCases, o => o.Ignore())
            .ReverseMap();
        
        CreateMap<UserStory, UserStoryResponseDto>();
        CreateMap<UserStoryForCreationDto, UserStory>();
        CreateMap<UserStoryForUpdateDto, UserStory>();
    }
}