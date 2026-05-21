using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IProjectService
{
    public Task<ProjectResponseDto> GetById(Guid id, CancellationToken ct);
    Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct);
    public Task<ProjectResponseDto> Create(ProjectForCreationDto project, Guid organizationId, CancellationToken ct);
    public Task Update(ProjectForUpdateDto project, CancellationToken ct);
    public Task<Guid> Refine(Guid projectId, CancellationToken ct);
    public Task MarkAsFinished(Guid projectId, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
}