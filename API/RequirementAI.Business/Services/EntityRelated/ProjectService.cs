using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class ProjectService(
    IProjectRepository projectRepository,
    IMapper mapper)
    : IProjectService
{
    public async Task<ProjectResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, ct);
        return mapper.Map<ProjectResponseDto>(entity);
    }

    public async Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct)
    {
        var entities = await projectRepository.GetByOrganization(organizationId, ct);
        return mapper.Map<List<ProjectResponseDto>>(entities);
    }

    public async Task<ProjectResponseDto> Create(ProjectForCreationDto project, CancellationToken ct)
    {
        var entity = mapper.Map<Project>(project);

        var created = await projectRepository.Create(entity, ct);

        return mapper.Map<ProjectResponseDto>(created);
    }

    public async Task Update(ProjectForUpdateDto project, CancellationToken ct)
    {
        var entity = mapper.Map<Project>(project);

        await projectRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, ct);
        await projectRepository.Delete(entity, ct);
    }
}