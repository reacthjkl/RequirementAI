using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Providers;

public class LocalAuthProvider(IUserRepository userRepository, IPasswordHasher hasher)
    : ILocalAuthProvider
{
    public async Task<User?> GetUserByValidCredentials(LocalAuthRequestDto request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, ct);

        if (user is not { Provider: AuthProvider.Local, Password: not null }) return null;

        return hasher.Verify(request.Password, user.Password) ? user : null;
    }
}