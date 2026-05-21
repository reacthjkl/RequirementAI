using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IProjectService
{
    Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct);
}