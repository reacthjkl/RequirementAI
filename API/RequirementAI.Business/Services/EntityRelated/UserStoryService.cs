using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class UserStoryService(
    IUserStoryRepository userStoryRepository,
    IScenarioRepository scenarioRepository,
    IMapper mapper)
    : IUserStoryService
{
    public async Task<UserStoryResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await userStoryRepository.GetById(id, organizationId, ct);
        return mapper.Map<UserStoryResponseDto>(entity);
    }

    public async Task<List<UserStoryResponseDto>> GetByScenarioId(Guid scenarioId, Guid organizationId, CancellationToken ct)
    {
        var entities = await userStoryRepository.GetByScenario(scenarioId, organizationId, ct);
        return mapper.Map<List<UserStoryResponseDto>>(entities);
    }

    public async Task<List<UserStoryResponseDto>> GetByPersonaId(Guid personaId, Guid organizationId, CancellationToken ct)
    {
        var entities = await userStoryRepository.GetByPersona(personaId, organizationId, ct);
        return mapper.Map<List<UserStoryResponseDto>>(entities);
    }

    public async Task<List<UserStoryResponseDto>> GetByProject(Guid projectId, Guid organizationId, CancellationToken ct)
    {
        var entities = await userStoryRepository.GetByProject(projectId, organizationId, ct);
        return mapper.Map<List<UserStoryResponseDto>>(entities);    }

    public async Task<UserStoryResponseDto> Create(UserStoryForCreationDto userStory, Guid organizationId, CancellationToken ct)
    {
        await scenarioRepository.GetById(userStory.ScenarioId, organizationId, ct);

        var entity = mapper.Map<UserStory>(userStory);

        var created = await userStoryRepository.Create(entity, ct);

        return mapper.Map<UserStoryResponseDto>(created);
    }

    public async Task Update(UserStoryForUpdateDto userStory, Guid organizationId, CancellationToken ct)
    {
        var entity = await userStoryRepository.GetById(userStory.Id, organizationId, ct);
        
        mapper.Map(userStory, entity);

        await userStoryRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await userStoryRepository.GetById(id, organizationId, ct);
        await userStoryRepository.Delete(entity, ct);
    }
}
