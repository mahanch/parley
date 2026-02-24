using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.ServerAgg.Entities;

/// <summary>
/// Represents a member of a server with associated roles.
/// This is an Entity within the Server aggregate.
/// </summary>
public class ServerMember : EntityBase<Guid>
{
    /// <summary>
    /// The ID of the server this member belongs to.
    /// </summary>
    public Guid ServerId { get; private set; }

    /// <summary>
    /// The ID of the user who is a member.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Custom nickname for this member in the server (if any).
    /// </summary>
    public string? Nickname { get; private set; }

    /// <summary>
    /// Private list of role IDs assigned to this member.
    /// </summary>
    private readonly List<Guid> _roleIds = new();

    /// <summary>
    /// Gets the read-only list of role IDs assigned to this member.
    /// </summary>
    public IReadOnlyList<Guid> RoleIds => _roleIds.AsReadOnly();

    /// <summary>
    /// Timestamp when this member joined the server.
    /// </summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>
    /// Timestamp when this member was last active.
    /// </summary>
    public DateTime? LastActiveAt { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private ServerMember()
    {
    }

    /// <summary>
    /// Creates a new ServerMember.
    /// </summary>
    public ServerMember(Guid serverId, Guid userId, IEnumerable<Guid>? initialRoles = null)
    {
        if (serverId == Guid.Empty)
            throw new ArgumentException("Server ID cannot be empty.", nameof(serverId));

        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        ServerId = serverId;
        UserId = userId;
        JoinedAt = DateTime.UtcNow;

        if (initialRoles != null)
        {
            _roleIds.AddRange(initialRoles);
        }
    }

    /// <summary>
    /// Updates the nickname for this member.
    /// </summary>
    public void UpdateNickname(string? nickname)
    {
        if (!string.IsNullOrWhiteSpace(nickname) && nickname.Length > 32)
            throw new ArgumentException("Nickname cannot exceed 32 characters.", nameof(nickname));

        Nickname = nickname;
    }

    /// <summary>
    /// Assigns a role to this member.
    /// </summary>
    public void AssignRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new ArgumentException("Role ID cannot be empty.", nameof(roleId));

        if (_roleIds.Contains(roleId))
            return; // Role already assigned

        _roleIds.Add(roleId);
    }

    /// <summary>
    /// Removes a role from this member.
    /// </summary>
    public void RemoveRole(Guid roleId)
    {
        _roleIds.Remove(roleId);
    }

    /// <summary>
    /// Updates the last active timestamp.
    /// </summary>
    public void UpdateLastActive()
    {
        LastActiveAt = DateTime.UtcNow;
    }
}

