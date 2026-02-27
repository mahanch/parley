using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.ServerAgg;
using Parley.Domain.Aggregates.ServerAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Server aggregate.
/// </summary>
public sealed class ServerRepository : RepositoryBase<Server, Guid>, IServerRepository
{
    public ServerRepository(ParleyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Server>> GetUserServersAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Servers
            .Where(s => s.Members.Any(m => m.UserId == userId))
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Server?> GetWithRolesAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Servers
            .Include(s => s.Roles)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Server?> GetWithMembersAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Servers
            .Include(s => s.Members)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Server?> GetWithRolesAndMembersAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Servers
            .Include(s => s.Roles)
            .Include(s => s.Members)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<bool> IsUserMemberAsync(
        Guid serverId, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await Context.ServerMembers
            .AnyAsync(m => m.ServerId == serverId && m.UserId == userId, cancellationToken);
    }
}

