using Microsoft.Extensions.Options;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Contract.Settings;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class ExternalAuthService : IExternalAuthService
{
    private readonly ICookiesHelper _cookiesHelper;
    private readonly IReadOnlyDictionary<AuthProvider, IExternalAuthProvider> _externalProviders;
    private readonly IJwtTokenService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepository _userRepo;

    public ExternalAuthService(
        IEnumerable<IExternalAuthProvider> providers,
        IUserRepository userRepo,
        IJwtTokenService jwtService,
        IOptions<JwtSettings> jwtSettings,
        ICookiesHelper cookiesHelper)
    {
        _externalProviders = providers.ToDictionary(p => p.Provider);
        _userRepo = userRepo;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _cookiesHelper = cookiesHelper;
    }

    public async Task AuthenticateExternalAsync(ExternalAuthRequestDto request, CancellationToken ct)
    {
        if (!_externalProviders.TryGetValue(request.Provider, out var provider))
            throw new AuthorizationException($"Unsupported provider: {request.Provider}");

        var payload = await provider.ValidateAsync(request.Token);
        if (payload == null)
            throw new AuthorizationException("Invalid token");

        var user = await _userRepo.GetByEmailAsync(payload.Email, ct)
                   ?? await _userRepo.CreateAsync(new User
                   {
                       Email = payload.Email,
                       Name = payload.Name,
                       Provider = request.Provider,
                       ProviderId = payload.ProviderId,
                       AvatarUrl = payload.AvatarUrl
                   }, ct);

        var token = _jwtService.GenerateJwt(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays);
        await _userRepo.UpdateAsync(user, ct);

        _cookiesHelper.SetAccessTokenCookie(token, _jwtSettings.AccessTokenLifetimeMinutes);
        _cookiesHelper.SetRefreshTokenCookie(refreshToken, _jwtSettings.RefreshTokenLifetimeDays);
    }
}