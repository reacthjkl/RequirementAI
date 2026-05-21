using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IPersonaService
{
    public Task<PersonaResponseDto> GetById(Guid id, CancellationToken ct);
    public Task<List<PersonaResponseDto>> GetByProjectId(Guid projectId, CancellationToken ct);
    public Task<PersonaResponseDto> Create(PersonaForCreationDto persona, CancellationToken ct);
    public Task Update(PersonaForUpdateDto persona, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
}