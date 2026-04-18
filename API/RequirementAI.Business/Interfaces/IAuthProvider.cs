using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces;

public interface IAuthProvider
{
    Task<User?> GetUserByValidCredentials(AuthRequestDto request, CancellationToken ct);
}