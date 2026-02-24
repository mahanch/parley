namespace Parley.Domain.Aggregates.ConversationAgg.Enums;

/// <summary>
/// Represents the type of a conversation.
/// </summary>
public enum ConversationType
{
    /// <summary>
    /// Direct message between two users.
    /// </summary>
    Direct = 0,

    /// <summary>
    /// Group conversation with multiple users.
    /// </summary>
    Group = 1,

    /// <summary>
    /// Channel within a server.
    /// </summary>
    ServerChannel = 2
}

