using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class ProjectService(
    IProjectRepository projectRepository,
    IProjectRefinementJobRepository projectRefinementJobRepository,
    IProjectStatusEnricher projectStatusEnricher,
    IMapper mapper)
    : IProjectService
{
    public async Task<ProjectResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, ct);
        
        var dto =  mapper.Map<ProjectResponseDto>(entity);
        
        await projectStatusEnricher.EnrichAsync(dto, ct);
        
        return dto;
    }

    public async Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct)
    {
        var entities = await projectRepository.GetByOrganization(organizationId, ct);
        
        var dtos = mapper.Map<List<ProjectResponseDto>>(entities);
        
        await projectStatusEnricher.EnrichRangeAsync(dtos, ct);
        
        return dtos;
    }

    public async Task<ProjectResponseDto> Create(ProjectForCreationDto project, Guid organizationId,
        CancellationToken ct)
    {
        var entity = mapper.Map<Project>(project);

        entity.OrganizationId = organizationId;

        var created = await projectRepository.Create(entity, ct);

        return mapper.Map<ProjectResponseDto>(created);
    }

    public async Task Update(ProjectForUpdateDto project, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(project.Id, ct);
        
        mapper.Map(project, entity);

        await projectRepository.Update(entity, ct);
    }

    public async Task<Guid> Refine(Guid projectId, CancellationToken ct)
    {
        var job = await projectRefinementJobRepository.Create(
            new ProjectRefinementJob
            {
                ProjectId = projectId
            }, 
            ct);
        
        return job.Id;
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, ct);
        await projectRepository.Delete(entity, ct);
    }
}