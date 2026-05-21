using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class UserStoryService(
    IUserStoryRepository userStoryRepository,
    IMapper mapper)
    : IUserStoryService
{
    public async Task<UserStoryResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await userStoryRepository.GetById(id, ct);
        return mapper.Map<UserStoryResponseDto>(entity);
    }

    public async Task<List<UserStoryResponseDto>> GetByScenarioId(Guid scenarioId, CancellationToken ct)
    {
        var entities = await userStoryRepository.GetByScenario(scenarioId, ct);
        return mapper.Map<List<UserStoryResponseDto>>(entities);
    }

    public async Task<List<UserStoryResponseDto>> GetByPersonaId(Guid personaId, CancellationToken ct)
    {
        var entities = await userStoryRepository.GetByPersona(personaId, ct);
        return mapper.Map<List<UserStoryResponseDto>>(entities);
    }

    public async Task<List<UserStoryResponseDto>> GetByProject(Guid projectId, CancellationToken ct)
    {
        var entities = await userStoryRepository.GetByProject(projectId, ct);
        return mapper.Map<List<UserStoryResponseDto>>(entities);    }

    public async Task<UserStoryResponseDto> Create(UserStoryForCreationDto userStory, CancellationToken ct)
    {
        var entity = mapper.Map<UserStory>(userStory);

        var created = await userStoryRepository.Create(entity, ct);

        return mapper.Map<UserStoryResponseDto>(created);
    }

    public async Task Update(UserStoryForUpdateDto userStory, CancellationToken ct)
    {
        var entity = mapper.Map<UserStory>(userStory);

        await userStoryRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await userStoryRepository.GetById(id, ct);
        await userStoryRepository.Delete(entity, ct);
    }
}