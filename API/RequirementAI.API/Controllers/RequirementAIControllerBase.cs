using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RequirementAI.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RequirementAIControllerBase : ControllerBase
{
    protected Guid UserId => GetClaimValue(nameof(UserId));
    protected Guid OrganizationId => GetClaimValue(nameof(OrganizationId));
    
    private Guid GetClaimValue(string key) =>  Guid.TryParse(User.FindFirstValue(key), out var id) ? id : Guid.Empty;
}