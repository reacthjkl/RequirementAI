using RequirementAI.Contract.Dto.AuthDtos;

namespace RequirementAI.Business.Interfaces;

public interface IExternalAuthService
{
    Task AuthenticateExternalAsync(ExternalAuthRequestDto request, CancellationToken ct);
}