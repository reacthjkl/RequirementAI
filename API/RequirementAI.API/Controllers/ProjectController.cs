using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class ProjectController(IProjectService projectService, IQualityScoreService qualityScoreService)
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
    
    [HttpGet("{projectId:guid}/overview")]
    public async Task<IActionResult> GetProjectQualityOverview(Guid projectId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetProjectQualityOverview(projectId, ct);
        return Ok(ResponseDto<ProjectQualityOverviewDto>.Success(result));
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
    
    [HttpPut("refine/{projectId:guid}")]
    public async Task<IActionResult> Refine([FromRoute] Guid projectId, [FromBody] string? customInstructions, CancellationToken ct)
    {
        await projectService.Refine(projectId, customInstructions, ct);
        return Ok(ResponseDto.Success());
    }
    
    [HttpPut("analyze/{projectId:guid}")]
    public async Task<IActionResult> Analyze([FromRoute] Guid projectId, CancellationToken ct)
    {
        var jobId = await projectService.Analyze(projectId, ct);
        return Ok(ResponseDto<Guid>.Success(jobId));
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await projectService.Delete(id, ct);
        return Ok(ResponseDto.Success());
    }
}