using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IAcceptanceCriteriaService
{
    public Task<AcceptanceCriteriaResponseDto> GetById(Guid id, CancellationToken ct);
    public Task<List<AcceptanceCriteriaResponseDto>> GetByUserStoryId(Guid userStoryId, CancellationToken ct);
    public Task<AcceptanceCriteriaResponseDto> Create(AcceptanceCriteriaForCreationDto acceptanceCriteria, CancellationToken ct);
    public Task Update(AcceptanceCriteriaForUpdateDto acceptanceCriteria, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
}