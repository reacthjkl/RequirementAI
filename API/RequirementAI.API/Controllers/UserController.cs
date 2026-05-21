using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class UserController(IUserService userSvc) : RequirementAIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser(CancellationToken ct)
    {
        var user = await userSvc.GetById(UserId, ct);
        return Ok(ResponseDto<UserDto>.Success(user));
    }
}