using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IProjectService
{
    public Task<ProjectResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct);
    public Task<ProjectWithArtifactsDto> GetWithArtifacts(Guid id, Guid organizationId, CancellationToken ct);
    public Task<ProjectWordCountDto> GetWordCounts(Guid id, Guid organizationId, CancellationToken ct);
    public Task<ProjectUserStoryDetailCountDto> GetUserStoryDetailCounts(
        Guid id,
        Guid organizationId,
        CancellationToken ct);
    Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct);
    public Task<ProjectResponseDto> Create(ProjectForCreationDto project, Guid organizationId, CancellationToken ct);
    public Task Update(ProjectForUpdateDto project, Guid organizationId, CancellationToken ct);
    public Task<Guid> Refine(Guid projectId, Guid organizationId, RefineRequestDto request, CancellationToken ct);
    public Task<Guid> Analyze(Guid projectId, Guid organizationId, CancellationToken ct);
    public Task Delete(Guid id, Guid organizationId, CancellationToken ct);
}
