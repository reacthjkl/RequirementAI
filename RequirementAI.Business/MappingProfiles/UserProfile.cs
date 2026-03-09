using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<LocalRegisterRequestDto, User>()
            .ForMember(d => d.Provider, o => o.MapFrom(_ => AuthProvider.Local));
    }
}