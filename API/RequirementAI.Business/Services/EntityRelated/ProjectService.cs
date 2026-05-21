using System.Text;
using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class ProjectService(
    IProjectRepository projectRepository,
    IProjectRefinementJobRepository projectRefinementJobRepository,
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
        var entity = mapper.Map<Project>(project);

        await projectRepository.Update(entity, ct);
    }

    public async Task<Guid> Refine(Guid projectId, CancellationToken ct)
    {
        var job = await projectRefinementJobRepository.Create(new ProjectRefinementJob { ProjectId = projectId }, ct);
        
        var project = await projectRepository.GetById(projectId, ct);
        project.Status = ProjectStatus.RefinementInProgress;
        await projectRepository.Update(project, ct);
        
        return job.Id;
    }

    public async Task MarkAsFinished(Guid projectId, CancellationToken ct)
    {
        var entity = await projectRepository.GetFullProjectById(projectId, ct);

        var isReady = entity.Personas.All(p => p.Scenarios.All(s => s.UserStories.Any()));
        
        if(!isReady) throw new BusinessException("Project is still incomplete");

        entity.Status = ProjectStatus.ReadyForRefinement;
        
        await projectRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, ct);
        await projectRepository.Delete(entity, ct);
    }
}