using AutoMapper;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto> GetById(Guid userId, CancellationToken ct)
    {
        var user = await userRepository.GetById(userId, ct);
        return mapper.Map<UserDto>(user);
    }

    public async Task<bool> IsEmailAvailable(string email, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailIgnoringFilters(email, ct);
        return user == null;
    }
}