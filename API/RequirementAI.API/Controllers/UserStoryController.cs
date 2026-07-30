using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class UserStoryController(IUserStoryService userStoryService)
    : RequirementAIControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await userStoryService.GetById(id, OrganizationId, ct);
        return Ok(ResponseDto<UserStoryResponseDto>.Success(result));
    }

    [HttpGet("by-scenario/{scenarioId:guid}")]
    public async Task<IActionResult> GetByScenarioId(Guid scenarioId, CancellationToken ct)
    {
        var result = await userStoryService.GetByScenarioId(scenarioId, OrganizationId, ct);
        return Ok(ResponseDto<List<UserStoryResponseDto>>.Success(result));
    }

    [HttpGet("by-persona/{personaId:guid}")]
    public async Task<IActionResult> GetByPersonaId(Guid personaId, CancellationToken ct)
    {
        var result = await userStoryService.GetByPersonaId(personaId, OrganizationId, ct);
        return Ok(ResponseDto<List<UserStoryResponseDto>>.Success(result));
    }

    [HttpGet("by-project/{projectId:guid}")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
    {
        var result = await userStoryService.GetByProject(projectId, OrganizationId, ct);
        return Ok(ResponseDto<List<UserStoryResponseDto>>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserStoryForCreationDto dto, CancellationToken ct)
    {
        var result = await userStoryService.Create(dto, OrganizationId, ct);
        return Ok(ResponseDto<UserStoryResponseDto>.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserStoryForUpdateDto dto, CancellationToken ct)
    {
        await userStoryService.Update(dto, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await userStoryService.Delete(id, OrganizationId, ct);
        return Ok(ResponseDto.Success());
    }
}
