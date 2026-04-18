namespace RequirementAI.Business.Interfaces;

public interface ICookiesHelper
{
    public void SetRefreshTokenCookie(string refreshToken, int ttlDays);
    public void SetAccessTokenCookie(string accessToken, int ttlMinutes);
    public void ResetTokenCookies();
}