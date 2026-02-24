using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ServerAgg.Enums;
using Parley.Domain.Aggregates.ServerAgg.Exceptions;

namespace Parley.Domain.Aggregates.ServerAgg.Entities;

/// <summary>
/// Represents a server/guild (Aggregate Root).
/// </summary>
public class Server : AggregateRoot<Guid>
{
    /// <summary>
    /// The name of this server.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Description of this server.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// The ID of the user who owns this server.
    /// </summary>
    public Guid OwnerId { get; private set; }

    /// <summary>
    /// URL to the server's icon image.
    /// </summary>
    public string? IconUrl { get; private set; }

    /// <summary>
    /// Private list of members in this server.
    /// </summary>
    private readonly List<ServerMember> _members = new();

    /// <summary>
    /// Gets the read-only list of members.
    /// </summary>
    public IReadOnlyList<ServerMember> Members => _members.AsReadOnly();

    /// <summary>
    /// Private list of roles in this server.
    /// </summary>
    private readonly List<ServerRole> _roles = new();

    /// <summary>
    /// Gets the read-only list of roles.
    /// </summary>
    public IReadOnlyList<ServerRole> Roles => _roles.AsReadOnly();

    /// <summary>
    /// Indicates whether this server is public and discoverable.
    /// </summary>
    public bool IsPublic { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private Server()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Creates a new Server.
    /// </summary>
    public Server(string name, Guid ownerId, string? description = null, bool isPublic = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Server name cannot be empty.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Server name cannot exceed 100 characters.", nameof(name));

        if (ownerId == Guid.Empty)
            throw new ArgumentException("Owner ID cannot be empty.", nameof(ownerId));

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        OwnerId = ownerId;
        IsPublic = isPublic;

        // Add the owner as a member automatically
        AddMember(ownerId);
    }

    /// <summary>
    /// Updates the server name.
    /// </summary>
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Server name cannot be empty.", nameof(newName));

        if (newName.Length > 100)
            throw new ArgumentException("Server name cannot exceed 100 characters.", nameof(newName));

        Name = newName;
        MarkAsModified();
    }

    /// <summary>
    /// Updates the server description.
    /// </summary>
    public void UpdateDescription(string? description)
    {
        Description = description;
        MarkAsModified();
    }

    /// <summary>
    /// Updates the server icon URL.
    /// </summary>
    public void UpdateIcon(string? iconUrl)
    {
        IconUrl = iconUrl;
        MarkAsModified();
    }

    /// <summary>
    /// Updates the public status of this server.
    /// </summary>
    public void UpdatePublicStatus(bool isPublic)
    {
        IsPublic = isPublic;
        MarkAsModified();
    }

    /// <summary>
    /// Adds a member to this server.
    /// </summary>
    public void AddMember(Guid userId, IEnumerable<Guid>? roleIds = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        if (_members.Any(m => m.UserId == userId))
            return; // Member already exists

        var member = new ServerMember(Id, userId, roleIds);
        _members.Add(member);
        MarkAsModified();
    }

    /// <summary>
    /// Removes a member from this server.
    /// </summary>
    public void RemoveMember(Guid userId)
    {
        if (userId == OwnerId)
            throw new ServerException("Cannot remove the server owner.");

        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member != null)
        {
            _members.Remove(member);
            MarkAsModified();
        }
    }

    /// <summary>
    /// Adds a role to this server.
    /// </summary>
    public void AddRole(string name, ServerPermissions permissions, int position = 0, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));

        var role = new ServerRole(Id, name, permissions, position, color);
        _roles.Add(role);
        MarkAsModified();
    }

    /// <summary>
    /// Removes a role from this server.
    /// </summary>
    public void RemoveRole(Guid roleId)
    {
        var role = _roles.FirstOrDefault(r => r.Id == roleId);
        if (role != null)
        {
            _roles.Remove(role);
            MarkAsModified();
        }
    }

    /// <summary>
    /// Transfers ownership of this server to another user.
    /// </summary>
    public void TransferOwnership(Guid newOwnerId)
    {
        if (newOwnerId == Guid.Empty)
            throw new ArgumentException("New owner ID cannot be empty.", nameof(newOwnerId));

        if (!_members.Any(m => m.UserId == newOwnerId))
            throw new ServerException("New owner must be a member of the server.");

        OwnerId = newOwnerId;
        MarkAsModified();
    }
}

