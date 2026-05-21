using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class OrganizationController(IOrganizationService organizationService)
    : RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await organizationService.GetById(id, ct);
        return Ok(ResponseDto<OrganizationResponseDto>.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrganizationForUpdateDto dto, CancellationToken ct)
    {
        await organizationService.Update(dto, ct);
        return Ok(ResponseDto.Success());
    }
}