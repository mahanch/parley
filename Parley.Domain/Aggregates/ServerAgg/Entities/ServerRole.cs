using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ServerAgg.Enums;

namespace Parley.Domain.Aggregates.ServerAgg.Entities;

/// <summary>
/// Represents a role within a server.
/// This is an Entity within the Server aggregate.
/// </summary>
public class ServerRole : EntityBase<Guid>
{
    /// <summary>
    /// The ID of the server this role belongs to.
    /// </summary>
    public Guid ServerId { get; private set; }

    /// <summary>
    /// The name of this role.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The permissions granted by this role (bitwise flags).
    /// </summary>
    public ServerPermissions Permissions { get; private set; }

    /// <summary>
    /// The color associated with this role (hex format, e.g., #FF5733).
    /// </summary>
    public string? Color { get; private set; }

    /// <summary>
    /// The position of this role in the hierarchy (lower number = higher in hierarchy).
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private ServerRole()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Creates a new ServerRole.
    /// </summary>
    public ServerRole(Guid serverId, string name, ServerPermissions permissions, int position = 0, string? color = null)
    {
        if (serverId == Guid.Empty)
            throw new ArgumentException("Server ID cannot be empty.", nameof(serverId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Role name cannot exceed 100 characters.", nameof(name));

        ServerId = serverId;
        Name = name;
        Permissions = permissions;
        Position = position;
        Color = color;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Updates the name of this role.
    /// </summary>
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Role name cannot be empty.", nameof(newName));

        if (newName.Length > 100)
            throw new ArgumentException("Role name cannot exceed 100 characters.", nameof(newName));

        Name = newName;
    }

    /// <summary>
    /// Updates the permissions for this role.
    /// </summary>
    public void UpdatePermissions(ServerPermissions newPermissions)
    {
        Permissions = newPermissions;
    }

    /// <summary>
    /// Updates the color for this role.
    /// </summary>
    public void UpdateColor(string? color)
    {
        Color = color;
    }

    /// <summary>
    /// Updates the position of this role in the hierarchy.
    /// </summary>
    public void UpdatePosition(int position)
    {
        Position = position;
    }

    /// <summary>
    /// Checks if this role has a specific permission.
    /// </summary>
    public bool HasPermission(ServerPermissions permission)
    {
        return (Permissions & permission) == permission;
    }

    /// <summary>
    /// Grants a specific permission to this role.
    /// </summary>
    public void GrantPermission(ServerPermissions permission)
    {
        Permissions |= permission;
    }

    /// <summary>
    /// Revokes a specific permission from this role.
    /// </summary>
    public void RevokePermission(ServerPermissions permission)
    {
        Permissions &= ~permission;
    }
}

