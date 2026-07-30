using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IScenarioService
{
    public Task<ScenarioResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct);
    Task<List<ScenarioResponseDto>> GetByPersonaId(Guid personaId, Guid organizationId, CancellationToken ct);
    public Task<ScenarioResponseDto> Create(ScenarioForCreationDto scenario, Guid organizationId, CancellationToken ct);
    public Task Update(ScenarioForUpdateDto scenario, Guid organizationId, CancellationToken ct);
    public Task Delete(Guid id, Guid organizationId, CancellationToken ct);
}
