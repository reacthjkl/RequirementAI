using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class ScenarioService(
    IScenarioRepository scenarioRepository,
    IMapper mapper)
    : IScenarioService
{
    public async Task<ScenarioResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await scenarioRepository.GetById(id, ct);
        return mapper.Map<ScenarioResponseDto>(entity);
    }

    public async Task<List<ScenarioResponseDto>> GetByPersonaId(Guid personaId, CancellationToken ct)
    {
        var entities = await scenarioRepository.GetByPersona(personaId, ct);
        return mapper.Map<List<ScenarioResponseDto>>(entities);
    }

    public async Task<ScenarioResponseDto> Create(ScenarioForCreationDto scenario, CancellationToken ct)
    {
        var entity = mapper.Map<Scenario>(scenario);

        var created = await scenarioRepository.Create(entity, ct);

        return mapper.Map<ScenarioResponseDto>(created);
    }

    public async Task Update(ScenarioForUpdateDto scenario, CancellationToken ct)
    {
        var entity = await scenarioRepository.GetById(scenario.Id, ct);
        
        mapper.Map(scenario, entity);

        await scenarioRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await scenarioRepository.GetById(id, ct);
        await scenarioRepository.Delete(entity, ct);
    }
}