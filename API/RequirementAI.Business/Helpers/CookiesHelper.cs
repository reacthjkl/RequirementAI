using Microsoft.AspNetCore.Http;
using RequirementAI.Business.Interfaces;

namespace RequirementAI.Business.Helpers;

public class CookiesHelper(IHttpContextAccessor httpContextAccessor) : ICookiesHelper
{
    private const string AccessTokenLabel = "access_token";
    private const string RefreshTokenLabel = "refresh_token";

    public void SetRefreshTokenCookie(string refreshToken, int ttlDays)
    {
        SetHttpOnlyToken(RefreshTokenLabel, refreshToken, DateTimeOffset.UtcNow.AddDays(ttlDays));
    }

    public void SetAccessTokenCookie(string accessToken, int ttlMinutes)
    {
        SetHttpOnlyToken(AccessTokenLabel, accessToken, DateTimeOffset.UtcNow.AddMinutes(ttlMinutes));
    }

    public void ResetTokenCookies()
    {
        SetHttpOnlyToken(AccessTokenLabel, "", DateTimeOffset.UtcNow.AddDays(-1));
        SetHttpOnlyToken(RefreshTokenLabel, "", DateTimeOffset.UtcNow.AddDays(-1));
    }

    private void SetHttpOnlyToken(string name, string value, DateTimeOffset expires)
    {
        if (httpContextAccessor.HttpContext?.Response.Cookies == null)
            throw new InvalidOperationException("HttpContext or Response.Cookies is not available.");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expires
        };

        httpContextAccessor.HttpContext.Response.Cookies.Append(name, value, cookieOptions);
    }
}