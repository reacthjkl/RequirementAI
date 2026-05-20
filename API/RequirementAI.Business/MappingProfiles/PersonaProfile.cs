using AutoMapper;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class PersonaProfile : Profile
{
    public PersonaProfile()
    {
        CreateMap<Persona, PersonaDto>().ReverseMap();
    }
}