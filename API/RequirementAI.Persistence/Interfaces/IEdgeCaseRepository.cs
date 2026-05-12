using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IEdgeCaseRepository
{
    Task<EdgeCase> GetById(Guid id, CancellationToken ct);
    Task<IList<EdgeCase>> GetByUserStoryId(Guid userStoryId, CancellationToken ct);
    Task<EdgeCase> Create(EdgeCase edgeCase, CancellationToken ct);
    Task<EdgeCase> Update(EdgeCase edgeCase, CancellationToken ct);
    Task Delete(EdgeCase edgeCase, CancellationToken ct);
}