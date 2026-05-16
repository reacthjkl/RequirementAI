using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Providers.Auth;

public class AuthProvider(IUserRepository userRepository, IPasswordHasher hasher)
    : IAuthProvider
{
    public async Task<User?> GetUserByValidCredentials(AuthRequestDto request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailIgnoringFilters(request.Email, ct);

        if (user is not {  Password: not null }) return null;

        return hasher.Verify(request.Password, user.Password) ? user : null;
    }
}