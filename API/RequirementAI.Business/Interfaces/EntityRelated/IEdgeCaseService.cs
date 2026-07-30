using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IEdgeCaseService
{
    Task<EdgeCaseResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct);

    Task<List<EdgeCaseResponseDto>> GetByUserStoryId(Guid userStoryId, Guid organizationId, CancellationToken ct);

    Task<EdgeCaseResponseDto> Create(EdgeCaseForCreationDto edgeCase, Guid organizationId, CancellationToken ct);

    Task Update(EdgeCaseForUpdateDto edgeCase, Guid organizationId, CancellationToken ct);

    Task Delete(Guid id, Guid organizationId, CancellationToken ct);
}
