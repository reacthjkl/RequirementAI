using RequirementAI.Persistence.Entities;

namespace RequirementAI.Persistence.Interfaces;

public interface IUserRepository
{
    Task<User> GetById(Guid id, CancellationToken ct);
    Task<User?> GetByEmailIgnoringFilters(string email, CancellationToken ct);
    Task<User?> GetByRefreshToken(string refreshToken, CancellationToken ct);
    Task<User> Create(User user, CancellationToken ct);
    Task Update(User user, CancellationToken ct);
}