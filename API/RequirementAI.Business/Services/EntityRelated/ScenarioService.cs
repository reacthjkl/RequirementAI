using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class ScenarioService(
    IScenarioRepository scenarioRepository,
    IPersonaRepository personaRepository,
    IMapper mapper)
    : IScenarioService
{
    public async Task<ScenarioResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await scenarioRepository.GetById(id, organizationId, ct);
        return mapper.Map<ScenarioResponseDto>(entity);
    }

    public async Task<List<ScenarioResponseDto>> GetByPersonaId(Guid personaId, Guid organizationId, CancellationToken ct)
    {
        var entities = await scenarioRepository.GetByPersona(personaId, organizationId, ct);
        return mapper.Map<List<ScenarioResponseDto>>(entities);
    }

    public async Task<ScenarioResponseDto> Create(ScenarioForCreationDto scenario, Guid organizationId, CancellationToken ct)
    {
        await personaRepository.GetById(scenario.PersonaId, organizationId, ct);

        var entity = mapper.Map<Scenario>(scenario);

        var created = await scenarioRepository.Create(entity, ct);

        return mapper.Map<ScenarioResponseDto>(created);
    }

    public async Task Update(ScenarioForUpdateDto scenario, Guid organizationId, CancellationToken ct)
    {
        var entity = await scenarioRepository.GetById(scenario.Id, organizationId, ct);
        
        mapper.Map(scenario, entity);

        await scenarioRepository.Update(entity, ct);
    }

    public async Task Delete(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await scenarioRepository.GetById(id, organizationId, ct);
        await scenarioRepository.Delete(entity, ct);
    }
}
