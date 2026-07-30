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
        var result = await qualityScoreService.GetPersonaQualityScores(personaId, OrganizationId, ct);
        return Ok(ResponseDto<List<PersonaQualityScoreDto>>.Success(result));
    }

    [HttpGet("by-scenario/{scenarioId:guid}")]
    public async Task<IActionResult> GetScenarioQualityScores(Guid scenarioId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetScenarioQualityScores(scenarioId, OrganizationId, ct);
        return Ok(ResponseDto<List<ScenarioQualityScoreDto>>.Success(result));
    }

    [HttpGet("by-user-story/{userStoryId:guid}")]
    public async Task<IActionResult> GetUserStoryQualityScores(Guid userStoryId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetUserStoryQualityScores(userStoryId, OrganizationId, ct);
        return Ok(ResponseDto<List<UserStoryQualityScoreDto>>.Success(result));
    }

    [HttpGet("latest/by-persona/{personaId:guid}")]
    public async Task<IActionResult> GetLatestPersonaQualityScore(Guid personaId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetLatestPersonaQualityScore(personaId, OrganizationId, ct);
        return Ok(ResponseDto<PersonaQualityScoreDto?>.Success(result));
    }

    [HttpGet("latest/by-scenario/{scenarioId:guid}")]
    public async Task<IActionResult> GetLatestScenarioQualityScore(Guid scenarioId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetLatestScenarioQualityScore(scenarioId, OrganizationId, ct);
        return Ok(ResponseDto<ScenarioQualityScoreDto?>.Success(result));
    }

    [HttpGet("latest/by-user-story/{userStoryId:guid}")]
    public async Task<IActionResult> GetLatestUserStoryQualityScore(Guid userStoryId, CancellationToken ct)
    {
        var result = await qualityScoreService.GetLatestUserStoryQualityScore(userStoryId, OrganizationId, ct);
        return Ok(ResponseDto<UserStoryQualityScoreDto?>.Success(result));
    }
}
