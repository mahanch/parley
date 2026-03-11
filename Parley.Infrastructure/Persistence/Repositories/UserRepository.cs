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
    
}