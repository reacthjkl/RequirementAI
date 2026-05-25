using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class QualityScoreController(IQualityScoreService qualityScoreService)
    : RequirementAIControllerBase
{
    [HttpGet("by-persona/{personaId:guid}")]
    public async Task<IActionResult> GetPersonaQualityScores(Guid personaId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetPersonaQualityScores(personaId, ct);
        return Ok(ResponseDto<List<PersonaQualityScoreDto>>.Success(result));
    }

    [HttpGet("by-scenario/{scenarioId:guid}")]
    public async Task<IActionResult> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetScenarioQualityScores(scenarioId, ct);
        return Ok(ResponseDto<List<ScenarioQualityScoreDto>>.Success(result));
    }

    [HttpGet("by-user-story/{userStoryId:guid}")]
    public async Task<IActionResult> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetUserStoryQualityScores(userStoryId, ct);
        return Ok(ResponseDto<List<UserStoryQualityScoreDto>>.Success(result));
    }
}