using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IUserStoryService
{
    public Task<UserStoryResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct);
    Task<List<UserStoryResponseDto>> GetByScenarioId(Guid scenarioId, Guid organizationId, CancellationToken ct);
    Task<List<UserStoryResponseDto>> GetByPersonaId(Guid personaId, Guid organizationId, CancellationToken ct);
    Task<List<UserStoryResponseDto>> GetByProject(Guid projectId, Guid organizationId, CancellationToken ct);
    public Task<UserStoryResponseDto> Create(UserStoryForCreationDto userStory, Guid organizationId, CancellationToken ct);
    public Task Update(UserStoryForUpdateDto userStory, Guid organizationId, CancellationToken ct);
    public Task Delete(Guid id, Guid organizationId, CancellationToken ct);
}
