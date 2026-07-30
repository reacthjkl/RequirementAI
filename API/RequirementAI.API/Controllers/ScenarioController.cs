using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class ScenarioController(IScenarioService scenarioService): RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var scenario = await scenarioService.GetById(id, OrganizationId, ct);
        return Ok(ResponseDto<ScenarioResponseDto>.Success(scenario));
    }

    [HttpGet("by-persona/{personaId:guid}")]
    public async Task<IActionResult> GetByPersonaId(Guid personaId, CancellationToken ct)
    {
        var scenarios = await scenarioService.GetByPersonaId(personaId, OrganizationId, ct);
        return Ok(ResponseDto<List<ScenarioResponseDto>>.Success(scenarios));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ScenarioForCreationDto scenario, CancellationToken ct)
    {
        var created = await scenarioService.Create(scenario, OrganizationId, ct);
        return Ok(ResponseDto<ScenarioResponseDto>.Success(created));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ScenarioForUpdateDto scenario, CancellationToken ct)
    {
        await scenarioService.Update(scenario, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await scenarioService.Delete(id, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }
}
