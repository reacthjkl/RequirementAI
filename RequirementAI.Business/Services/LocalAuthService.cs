using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Settings;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class LocalAuthService(
    ILocalAuthProvider provider,
    IUserRepository userRepository,
    IJwtTokenService jwtService,
    IOptions<JwtSettings> jwtSettings,
    ICookiesHelper cookiesHelper,
    ICurrentUserService currentUser,
    IMapper mapper,
    IConfiguration configuration,
    IEmailService emailService,
    IPasswordHasher passwordHasher
)
    : ILocalAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task AuthenticateLocalAsync(LocalAuthRequestDto request, CancellationToken ct)
    {
        var userInfo = await provider.ValidateAsync(request, ct);
        if (userInfo == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var user = await userRepository.GetByEmailAsync(userInfo.Email, ct);
        if (user == null)
            throw new UnauthorizedAccessException("User not found.");

        if (!user.EmailConfirmed)
            throw new UnauthorizedAccessException("Email was not confirmed.");

        var accessToken = jwtService.GenerateJwt(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);

        await userRepository.UpdateAsync(user, ct);


        cookiesHelper.SetAccessTokenCookie(accessToken, _jwtSettings.AccessTokenLifetimeMinutes);
        cookiesHelper.SetRefreshTokenCookie(refreshToken, _jwtSettings.RefreshTokenLifetimeDays);
    }

    public async Task RefreshTokens(string refreshToken, CancellationToken ct)
    {
        var user = await userRepository.GetUserByRefreshToken(refreshToken, ct)
                   ?? throw new KeyNotFoundException("Invalid refresh token used.");

        if (user.RefreshTokenExpiry <= DateTimeOffset.UtcNow)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await userRepository.UpdateAsync(user, ct);
            throw new SecurityTokenException("Expired refresh token used.");
        }

        var newAccessToken = jwtService.GenerateJwt(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);

        await userRepository.UpdateAsync(user, ct);

        cookiesHelper.SetAccessTokenCookie(newAccessToken, _jwtSettings.AccessTokenLifetimeMinutes);
        cookiesHelper.SetRefreshTokenCookie(newRefreshToken, _jwtSettings.RefreshTokenLifetimeDays);
    }

    public async Task LogoutAsync(CancellationToken ct)
    {
        var user = await userRepository.GetById(currentUser.Id, ct)
                   ?? throw new KeyNotFoundException("User not found.");

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await userRepository.UpdateAsync(user, ct);
        cookiesHelper.ResetTokenCookies();
    }

    public async Task Register(LocalRegisterRequestDto request, CancellationToken ct)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, ct);

        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = mapper.Map<User>(request);

        user.EmailConfirmationToken = Guid.NewGuid();
        user.EmailConfirmationTokenExpiry = DateTimeOffset.UtcNow.AddDays(1);

        if (user.Password != null)
            user.Password = passwordHasher.Hash(user.Password);

        await userRepository.CreateAsync(user, ct);

        var userIdBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString()));
        var tokenBase64 =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(user.EmailConfirmationToken.ToString() ?? string.Empty));

        var confirmLink =
            $"{configuration["Frontend:Url"]}/confirm-email?userId={userIdBase64}&token={tokenBase64}";

        await emailService.SendAsync(user.Email, "Confirm your email",
            $"Click to confirm: <a href='{confirmLink}'>{confirmLink}</a>", ct);
    }

    public async Task ConfirmEmail(Guid userId, Guid token, CancellationToken ct)
    {
        var user = await userRepository.GetById(userId, ct);

        if (user is null || user.EmailConfirmationToken != token)
            throw new UnauthorizedAccessException(
                "Link is invalid. Please request a new one. If the issue persists, contact support.");

        if (user.EmailConfirmationTokenExpiry < DateTimeOffset.UtcNow)
            throw new UnauthorizedAccessException("Link has expired. Please request a new one.");

        user.EmailConfirmed = true;
        user.EmailConfirmationToken = null;
        user.EmailConfirmationTokenExpiry = null;

        await userRepository.UpdateAsync(user, ct);
    }
}