using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

public class UserRepository:RepositoryBase<User,Guid>, IUserRepository
{
    private readonly ParleyDbContext _context;
    public UserRepository(ParleyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail, cancellationToken);
    }
}