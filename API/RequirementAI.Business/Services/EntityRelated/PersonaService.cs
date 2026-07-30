using AutoMapper;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class PersonaService(
    IPersonaRepository personaRepository,
    IProjectRepository projectRepository,
    IMapper mapper):IPersonaService
{
    public async Task<PersonaResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await personaRepository.GetById(id, organizationId, ct);
        return mapper.Map<PersonaResponseDto>(entity);
    }

    public async Task<List<PersonaResponseDto>> GetByProjectId(Guid projectId, Guid organizationId, CancellationToken ct)
    {
        var entities = await personaRepository.GetByProject(projectId, organizationId, ct);
        return mapper.Map<List<PersonaResponseDto>>(entities);
    }

    public async Task<PersonaResponseDto> Create(PersonaForCreationDto persona, Guid organizationId, CancellationToken ct)
    {
        await projectRepository.GetById(persona.ProjectId, organizationId, ct);

        var entity = mapper.Map<Persona>(persona);

        var created = await personaRepository.Create(entity, ct);

        return mapper.Map<PersonaResponseDto>(created);
    }

    public async Task Update(PersonaForUpdateDto persona, Guid organizationId, CancellationToken ct)
    {
        var existingPersona = await personaRepository.GetById(persona.Id, organizationId, ct);

        mapper.Map(persona, existingPersona);

        await personaRepository.Update(existingPersona, ct);
    }

    public async Task Delete(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await personaRepository.GetById(id, organizationId, ct);
        await personaRepository.Delete(entity, ct);
    }
}
