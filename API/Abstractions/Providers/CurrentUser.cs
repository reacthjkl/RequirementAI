using Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Abstractions.Providers;

public class CurrentUser(IHttpContextAccessor accessor): ICurrentUser
{
    public Guid? OrganizationId {
        get
        {
            var organizationId = accessor.HttpContext!.User.FindFirst("OrganizationId")?.Value;

            if (string.IsNullOrWhiteSpace(organizationId)) return null;
            
            return Guid.Parse(organizationId);
        }
    }
}