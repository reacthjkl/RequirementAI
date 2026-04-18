using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterRequestDto, User>();
    }
}
