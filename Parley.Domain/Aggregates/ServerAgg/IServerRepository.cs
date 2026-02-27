using Parley.Domain.Aggregates.ServerAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.ServerAgg;

/// <summary>
/// Repository interface for Server aggregate operations.
/// </summary>
public interface IServerRepository : IRepository<Server, Guid>
{
    /// <summary>
    /// Gets all servers a user is a member of.
    /// </summary>
    Task<IEnumerable<Server>> GetUserServersAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a server with its roles loaded.
    /// </summary>
    Task<Server?> GetWithRolesAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a server with its members loaded.
    /// </summary>
    Task<Server?> GetWithMembersAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a server with roles and members loaded.
    /// </summary>
    Task<Server?> GetWithRolesAndMembersAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user is a member of a server.
    /// </summary>
    Task<bool> IsUserMemberAsync(Guid serverId, Guid userId, CancellationToken cancellationToken = default);
}

