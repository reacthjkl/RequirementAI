using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class EdgeCaseController(IEdgeCaseService edgeCaseService)
    : RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await edgeCaseService.GetById(id, OrganizationId, ct);
        return Ok(ResponseDto<EdgeCaseResponseDto>.Success(result));
    }

    [HttpGet("by-user-story/{userStoryId:guid}")]
    public async Task<IActionResult> GetByUserStoryId(Guid userStoryId, CancellationToken ct)
    {
        var result = await edgeCaseService.GetByUserStoryId(userStoryId, OrganizationId, ct);
        return Ok(ResponseDto<List<EdgeCaseResponseDto>>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EdgeCaseForCreationDto dto, CancellationToken ct)
    {
        var result = await edgeCaseService.Create(dto, OrganizationId, ct);
        return Ok(ResponseDto<EdgeCaseResponseDto>.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EdgeCaseForUpdateDto dto, CancellationToken ct)
    {
        await edgeCaseService.Update(dto, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await edgeCaseService.Delete(id, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }
}
