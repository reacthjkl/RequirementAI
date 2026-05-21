using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class ProjectProfile: Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectResponseDto>();
        CreateMap<ProjectForCreationDto, Project>();
        CreateMap<ProjectForUpdateDto, Project>();
    }
}