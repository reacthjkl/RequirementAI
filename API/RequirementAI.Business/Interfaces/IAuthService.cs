using RequirementAI.Contract.Dto.AuthDtos;

namespace RequirementAI.Business.Interfaces;

public interface IAuthService
{
    Task AuthenticateLocalAsync(AuthRequestDto request, CancellationToken ct);
    Task RefreshTokens(string refreshToken, CancellationToken ct);
    Task LogoutAsync(Guid userId, CancellationToken ct);
    Task Register(RegisterRequestDto request, CancellationToken ct);
}