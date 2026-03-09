using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services;

public class UserService(IUserRepository userRepository, ICurrentUserService currentUser, IMapper mapper) : IUserService
{
    public async Task<UserDto> GetAsync(CancellationToken ct)
    {
        var user = await userRepository.GetById(currentUser.Id, ct)
                   ?? throw new KeyNotFoundException("User not found");

        return mapper.Map<UserDto>(user);
    }

    public async Task<bool> IsEmailAvailable(string email, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);
        return user == null;
    }
}