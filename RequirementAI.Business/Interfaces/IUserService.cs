using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IUserService
{
    Task<UserDto> GetById(Guid userId, CancellationToken ct);
    Task<bool> IsEmailAvailable(string email, CancellationToken ct);
}