using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IAcceptanceCriteriaService
{
    public Task<AcceptanceCriteriaResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct);
    public Task<List<AcceptanceCriteriaResponseDto>> GetByUserStoryId(Guid userStoryId, Guid organizationId, CancellationToken ct);
    public Task<AcceptanceCriteriaResponseDto> Create(AcceptanceCriteriaForCreationDto acceptanceCriteria, Guid organizationId, CancellationToken ct);
    public Task Update(AcceptanceCriteriaForUpdateDto acceptanceCriteria, Guid organizationId, CancellationToken ct);
    public Task Delete(Guid id, Guid organizationId, CancellationToken ct);
}
