using Parley.Domain.Aggregates.ServerAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.ServerAgg;

/// <summary>
/// Repository interface for Server aggregate operations.
/// Focused on write operations only (Commands).
/// Query operations use IContext directly (Queries).
/// </summary>
public interface IServerRepository : IRepository<Server, Guid>
{
    // Repository is now focused only on aggregate-root operations
    // Query methods moved to IContext for read efficiency
}

