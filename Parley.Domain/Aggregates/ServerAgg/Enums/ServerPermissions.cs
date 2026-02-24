namespace Parley.Domain.Aggregates.ServerAgg.Enums;

/// <summary>
/// Represents server-level permissions using bitwise flags.
/// This allows for efficient permission checking and storage.
/// </summary>
[Flags]
public enum ServerPermissions : ulong
{
    /// <summary>
    /// No permissions.
    /// </summary>
    None = 0,

    /// <summary>
    /// Permission to send messages in server channels.
    /// </summary>
    SendMessages = 1UL << 0,

    /// <summary>
    /// Permission to connect to voice channels.
    /// </summary>
    ConnectToVoice = 1UL << 1,

    /// <summary>
    /// Permission to speak in voice channels.
    /// </summary>
    Speak = 1UL << 2,

    /// <summary>
    /// Permission to mute other users.
    /// </summary>
    MuteMembers = 1UL << 3,

    /// <summary>
    /// Permission to deafen other users.
    /// </summary>
    DeafenMembers = 1UL << 4,

    /// <summary>
    /// Permission to move users between voice channels.
    /// </summary>
    MoveMembers = 1UL << 5,

    /// <summary>
    /// Permission to manage roles.
    /// </summary>
    ManageRoles = 1UL << 6,

    /// <summary>
    /// Permission to manage channels.
    /// </summary>
    ManageChannels = 1UL << 7,

    /// <summary>
    /// Permission to delete messages.
    /// </summary>
    DeleteMessages = 1UL << 8,

    /// <summary>
    /// Permission to manage server.
    /// </summary>
    Administrator = 1UL << 9
}

