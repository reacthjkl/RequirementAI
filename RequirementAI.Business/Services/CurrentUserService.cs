using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RequirementAI.Business.Interfaces;

namespace RequirementAI.Business.Services;

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public Guid Id
    {
        get
        {
            var idStr = contextAccessor.HttpContext?.User.FindFirstValue("UserId");
            return !Guid.TryParse(idStr, out var id)
                ? throw new UnauthorizedAccessException("Invalid or missing user ID in claims.")
                : id;
        }
    }
}