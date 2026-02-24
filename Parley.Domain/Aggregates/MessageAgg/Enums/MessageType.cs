namespace Parley.Domain.Aggregates.MessageAgg.Enums;

/// <summary>
/// Represents the type of a message.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Regular message.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Reply to another message.
    /// </summary>
    Reply = 1,

    /// <summary>
    /// System-generated message (e.g., user joined, left).
    /// </summary>
    System = 2
}

