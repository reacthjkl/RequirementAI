using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Providers;

public class LocalAuthProvider(IUserRepository userRepository, IPasswordHasher hasher, IMapper mapper)
    : ILocalAuthProvider
{
    public async Task<UserIdentityPayload?> ValidateAsync(LocalAuthRequestDto request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, ct);

        if (user is not { Provider: AuthProvider.Local }) return null;

        var valid = hasher.Verify(request.Password, user.Password!);

        return valid ? mapper.Map<UserIdentityPayload>(user) : null;
    }
}