using RequirementAI.Contract.Dto.AuthDtos;

namespace RequirementAI.Business.Interfaces;

public interface ILocalAuthService
{
    Task AuthenticateLocalAsync(LocalAuthRequestDto request, CancellationToken ct);
    Task RefreshTokens(string refreshToken, CancellationToken ct);
    Task LogoutAsync(CancellationToken ct);
    Task Register(LocalRegisterRequestDto request, CancellationToken ct);
    Task ConfirmEmail(Guid userId, Guid token, CancellationToken ct);
}