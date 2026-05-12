using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IAcceptanceCriteriaRepository
{
    Task<AcceptanceCriteria> GetById(Guid id, CancellationToken ct);
    Task<IList<AcceptanceCriteria>> GetByUserStoryId(Guid userStoryId, CancellationToken ct);
    Task<AcceptanceCriteria> Create(AcceptanceCriteria acceptanceCriteria, CancellationToken ct);
    Task<AcceptanceCriteria> Update(AcceptanceCriteria acceptanceCriteria, CancellationToken ct);
    Task Delete(AcceptanceCriteria acceptanceCriteria, CancellationToken ct);
}