using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class AcceptanceCriteriaController(IAcceptanceCriteriaService acceptanceCriteriaService)
    : RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await acceptanceCriteriaService.GetById(id, OrganizationId, ct);
        return Ok(ResponseDto<AcceptanceCriteriaResponseDto>.Success(result));
    }

    [HttpGet("by-user-story/{userStoryId:guid}")]
    public async Task<IActionResult> GetByUserStoryId(Guid userStoryId, CancellationToken ct)
    {
        var result = await acceptanceCriteriaService.GetByUserStoryId(userStoryId, OrganizationId, ct);
        return Ok(ResponseDto<List<AcceptanceCriteriaResponseDto>>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AcceptanceCriteriaForCreationDto dto, CancellationToken ct)
    {
        var result = await acceptanceCriteriaService.Create(dto, OrganizationId, ct);
        return Ok(ResponseDto<AcceptanceCriteriaResponseDto>.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AcceptanceCriteriaForUpdateDto dto, CancellationToken ct)
    {
        await acceptanceCriteriaService.Update(dto, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await acceptanceCriteriaService.Delete(id, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }
}
