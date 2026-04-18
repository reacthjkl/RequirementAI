using Microsoft.EntityFrameworkCore;
using RequirementAI.Contract.Exceptions;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Repositories;

public class UserRepository(RequirementAIContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<User> CreateAsync(User user, CancellationToken ct)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync(ct);
        return user;
    }

    public async Task UpdateAsync(User user, CancellationToken ct)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task<User?> GetUserByRefreshToken(string expiredRefreshToken, CancellationToken ct)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == expiredRefreshToken, ct);
    }

    public async Task<User> GetById(Guid id, CancellationToken ct)
    {
        return await context.Users
                   .AsNoTracking()
                   .FirstOrDefaultAsync(u => u.Id == id, ct)
               ?? throw new PersistenceException("User not found");
    }
}