using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class ProjectController(IProjectService projectService)
    : RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await projectService.GetById(id, ct);
        return Ok(ResponseDto<ProjectResponseDto>.Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetByOrganizationId(CancellationToken ct)
    {
        var result = await projectService.GetByOrganizationId(OrganizationId, ct);
        return Ok(ResponseDto<List<ProjectResponseDto>>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectForCreationDto dto, CancellationToken ct)
    {
        var result = await projectService.Create(dto, OrganizationId, ct);
        return Ok(ResponseDto<ProjectResponseDto>.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProjectForUpdateDto dto, CancellationToken ct)
    {
        await projectService.Update(dto, ct);
        return Ok(ResponseDto.Success());
    }
    
    [HttpPut("mark-as-finished/{id:guid}")]
    public async Task<IActionResult> MarkAsFinished([FromBody] Guid projectId, CancellationToken ct)
    {
        await projectService.MarkAsFinished(projectId, ct);
        return Ok(ResponseDto.Success());
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await projectService.Delete(id, ct);
        return Ok(ResponseDto.Success());
    }
}