using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IPersonaService
{
    public Task<PersonaResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct);
    public Task<List<PersonaResponseDto>> GetByProjectId(Guid projectId, Guid organizationId, CancellationToken ct);
    public Task<PersonaResponseDto> Create(PersonaForCreationDto persona, Guid organizationId, CancellationToken ct);
    public Task Update(PersonaForUpdateDto persona, Guid organizationId, CancellationToken ct);
    public Task Delete(Guid id, Guid organizationId, CancellationToken ct);
}
