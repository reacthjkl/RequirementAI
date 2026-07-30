using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IUserStoryRepository
{
    Task<UserStory> GetById(Guid id, CancellationToken ct);
    Task<UserStory> GetById(Guid id, Guid organizationId, CancellationToken ct);
    Task<IList<UserStory>> GetByScenario(Guid scenarioId, CancellationToken ct);
    Task<IList<UserStory>> GetByScenario(Guid scenarioId, Guid organizationId, CancellationToken ct);
    Task<IList<UserStory>> GetByPersona(Guid personaId, CancellationToken ct);
    Task<IList<UserStory>> GetByPersona(Guid personaId, Guid organizationId, CancellationToken ct);
    Task<IList<UserStory>> GetByProject(Guid projectId, CancellationToken ct);
    Task<IList<UserStory>> GetByProject(Guid projectId, Guid organizationId, CancellationToken ct);
    Task<UserStory> Create(UserStory userStory, CancellationToken ct);
    Task<UserStory> Update(UserStory userStory, CancellationToken ct);
    Task Delete(UserStory userStory, CancellationToken ct);
}
