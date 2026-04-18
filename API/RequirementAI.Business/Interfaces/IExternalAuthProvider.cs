using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;

namespace RequirementAI.Business.Interfaces;

public interface IExternalAuthProvider
{
    AuthProvider Provider { get; }
    Task<UserIdentityPayload?> ValidateAsync(string token);
}