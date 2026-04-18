using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IUserRepository
{
    Task<User> GetById(Guid id, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetUserByRefreshToken(string expiredRefreshToken, CancellationToken ct);
    Task<User> CreateAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
}