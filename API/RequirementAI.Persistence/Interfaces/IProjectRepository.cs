using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IProjectRepository
{
    Task<Project> GetById(Guid id, CancellationToken ct);
    Task<Project> GetById(Guid id, Guid organizationId, CancellationToken ct);
    Task<Dictionary<Guid, bool>> GetCompletenessByProjectIds(List<Guid> projectIds, CancellationToken ct);
    Task<Project> GetFullProjectById(Guid id, CancellationToken ct);
    Task<Project> GetFullProjectById(Guid id, Guid organizationId, CancellationToken ct);
    Task<DateTimeOffset> GetLatestContentUpdate(Guid id, CancellationToken ct);
    Task<IList<Project>> GetByOrganization(Guid organizationId, CancellationToken ct);
    Task<Project> Create(Project project, CancellationToken ct);
    Task<Project> Update(Project project, CancellationToken ct);
    Task Delete(Project project, CancellationToken ct);
}
