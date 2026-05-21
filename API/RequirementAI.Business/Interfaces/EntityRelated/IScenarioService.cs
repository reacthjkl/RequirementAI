using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IScenarioService
{
    public Task<ScenarioResponseDto> GetById(Guid id, CancellationToken ct);
    Task<List<ScenarioResponseDto>> GetByPersonaId(Guid personaId, CancellationToken ct);
    public Task<ScenarioResponseDto> Create(ScenarioForCreationDto scenario, CancellationToken ct);
    public Task Update(ScenarioForUpdateDto scenario, CancellationToken ct);
    public Task Delete(Guid id, CancellationToken ct);
}