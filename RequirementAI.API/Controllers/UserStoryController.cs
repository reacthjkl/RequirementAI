using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Services;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class UserStoryController(IUserStoryService userStoryService) : RequirementAIControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GenerateUserStories(string description, CancellationToken ct)
    {
        var stories = await userStoryService.Generate(description, ct);
        return Ok(ResponseDto<UserStoryDto>.SuccessList(stories));
    }
}