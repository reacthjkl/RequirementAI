using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RequirementAI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RequirementAIControllerBase : ControllerBase
{
    protected Guid UserId => Guid.TryParse(User.FindFirstValue("UserId"), out var id) ? id : Guid.Empty;
}