using RequirementAI.Contract.Dto.AuthDtos;

namespace RequirementAI.Business.Interfaces;

public interface ILocalAuthProvider
{
    Task<UserIdentityPayload?> ValidateAsync(LocalAuthRequestDto request, CancellationToken ct);
}