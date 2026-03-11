using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces;

public interface ILocalAuthProvider
{
    Task<User?> GetUserByValidCredentials(LocalAuthRequestDto request, CancellationToken ct);
}