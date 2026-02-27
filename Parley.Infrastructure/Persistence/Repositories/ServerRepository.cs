using Parley.Domain.Aggregates.ServerAgg;
using Parley.Domain.Aggregates.ServerAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Server aggregate.
/// Focused on write operations. Queries use IContext directly.
/// </summary>
public sealed class ServerRepository : RepositoryBase<Server, Guid>, IServerRepository
{
    public ServerRepository(ParleyDbContext context) : base(context)
    {
    }
}

