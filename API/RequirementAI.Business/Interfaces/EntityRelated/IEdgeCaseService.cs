using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IEdgeCaseService
{  Task<EdgeCaseResponseDto> GetById(Guid id, CancellationToken ct);

    Task<List<EdgeCaseResponseDto>> GetByUserStoryId(Guid userStoryId, CancellationToken ct);

    Task<EdgeCaseResponseDto> Create(EdgeCaseForCreationDto edgeCase, CancellationToken ct);

    Task Update(EdgeCaseForUpdateDto edgeCase, CancellationToken ct);

    Task Delete(Guid id, CancellationToken ct);
    
}