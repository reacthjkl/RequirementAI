using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class PersonaController(IPersonaService personaService)
    : RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await personaService.GetById(id, OrganizationId, ct);
        return Ok(ResponseDto<PersonaResponseDto>.Success(result));
    }

    [HttpGet("by-project/{projectId:guid}")]
    public async Task<IActionResult> GetByProjectId(Guid projectId, CancellationToken ct)
    {
        var result = await personaService.GetByProjectId(projectId, OrganizationId, ct);
        return Ok(ResponseDto<List<PersonaResponseDto>>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonaForCreationDto dto, CancellationToken ct)
    {
        var result = await personaService.Create(dto, OrganizationId, ct);
        return Ok(ResponseDto<PersonaResponseDto>.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PersonaForUpdateDto dto, CancellationToken ct)
    {
        await personaService.Update(dto, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await personaService.Delete(id, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }
}
