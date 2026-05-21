using RequirementAI.Contract.Dto;

namespace RequirementAI.Business.Interfaces.EntityRelated;

public interface IUserService
{
    Task<UserDto> GetById(Guid userId, CancellationToken ct);
    Task<bool> IsEmailAvailable(string email, CancellationToken ct);
}