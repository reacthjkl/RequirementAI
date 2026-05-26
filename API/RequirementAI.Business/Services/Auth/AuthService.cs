using System.Security.Authentication;
using AutoMapper;
using Microsoft.Extensions.Options;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Contract.Settings;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Auth;

public class AuthService(
    IAuthProvider provider,
    IUserRepository userRepository,
    IJwtTokenService jwtService,
    IOptions<JwtSettings> jwtSettings,
    ICookiesHelper cookiesHelper,
    IMapper mapper,
    IPasswordHasher passwordHasher
)
    : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task AuthenticateAsync(AuthRequestDto request, CancellationToken ct)
    {
        var user = await provider.GetUserByValidCredentials(request, ct)
                   ?? throw new AuthorizationException("Invalid email or password.");

        var accessToken = jwtService.GenerateJwt(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);

        await userRepository.Update(user, ct);

        cookiesHelper.SetAccessTokenCookie(accessToken, _jwtSettings.AccessTokenLifetimeMinutes);
        cookiesHelper.SetRefreshTokenCookie(refreshToken, _jwtSettings.RefreshTokenLifetimeDays);
    }

    public async Task RefreshTokens(string refreshToken, CancellationToken ct)
    {
        var user = await userRepository.GetByRefreshToken(refreshToken, ct)
                   ?? throw new AuthenticationException("Invalid refresh token used.");

        if (user.RefreshTokenExpiry <= DateTimeOffset.UtcNow)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await userRepository.Update(user, ct);
            throw new AuthenticationException("Expired refresh token used.");
        }

        var newAccessToken = jwtService.GenerateJwt(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);

        await userRepository.Update(user, ct);

        cookiesHelper.SetAccessTokenCookie(newAccessToken, _jwtSettings.AccessTokenLifetimeMinutes);
        cookiesHelper.SetRefreshTokenCookie(newRefreshToken, _jwtSettings.RefreshTokenLifetimeDays);
    }

    public async Task LogoutAsync(Guid userId, CancellationToken ct)
    {
        var user = await userRepository.GetById(userId, ct);

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await userRepository.Update(user, ct);
        cookiesHelper.ResetTokenCookies();
    }

    public async Task Register(RegisterRequestDto request, CancellationToken ct)
    {
        var existingUser = await userRepository.GetByEmailIgnoringFilters(request.Email, ct);

        if (existingUser != null)
            throw new BusinessException("User with this email already exists.");

        var user = mapper.Map<User>(request);

        if (user.Password != null)
            user.Password = passwordHasher.Hash(user.Password);

        await userRepository.Create(user, ct);
    }
}