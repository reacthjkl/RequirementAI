using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces;

public interface IUserService
{
    Task<UserDto> GetAsync(CancellationToken ct);
    Task<bool> IsEmailAvailable(string email, CancellationToken ct);
}