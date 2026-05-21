using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IUserStoryService
{
    public Task<UserStoryResponseDto> GetById(Guid id, CancellationToken ct);
    Task<List<UserStoryResponseDto>> GetByScenarioId(Guid scenarioId, CancellationToken ct);
    Task<List<UserStoryResponseDto>> GetByPersonaId(Guid personaId, CancellationToken ct);
    Task<List<UserStoryResponseDto>> GetByProject(Guid projectId, CancellationToken ct);
    public Task<UserStoryResponseDto> Create(UserStoryForCreationDto userStory, CancellationToken ct);
    public Task Update(UserStoryForUpdateDto userStory, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
}

