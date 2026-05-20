using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class TestController(
    ITestService testService)
    : RequirementAIControllerBase
{
    [AllowAnonymous]
    [HttpPost("test-persona-refinement")]
    public async Task<ActionResult<ResponseDto>> Login(
        CancellationToken ct = default)
    {
        await testService.TestPersonaRefinement(ct);
        return Ok(ResponseDto.Success());
    }
}