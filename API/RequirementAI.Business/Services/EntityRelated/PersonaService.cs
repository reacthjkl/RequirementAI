using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class PersonaService(IPersonaRepository personaRepository, IMapper mapper):IPersonaService
{
    public async Task<PersonaResponseDto> GetById(Guid id, CancellationToken ct)
    {
        var entity = await personaRepository.GetById(id, ct);
        return mapper.Map<PersonaResponseDto>(entity);
    }

    public async Task<List<PersonaResponseDto>> GetByProjectId(Guid projectId, CancellationToken ct)
    {
        var entities = await personaRepository.GetByProject(projectId, ct);
        return mapper.Map<List<PersonaResponseDto>>(entities);
    }

    public async Task<PersonaResponseDto> Create(PersonaForCreationDto persona, CancellationToken ct)
    {
        var entity = mapper.Map<Persona>(persona);

        var created = await personaRepository.Create(entity, ct);

        return mapper.Map<PersonaResponseDto>(created);
    }

    public async Task Update(PersonaForUpdateDto persona, CancellationToken ct)
    {
        var existingPersona = await personaRepository.GetById(persona.Id, ct);

        mapper.Map(persona, existingPersona);

        await personaRepository.Update(existingPersona, ct);
    }

    public async Task Delete(Guid id, CancellationToken ct)
    {
        var entity = await personaRepository.GetById(id, ct);
        await personaRepository.Delete(entity, ct);
    }
}