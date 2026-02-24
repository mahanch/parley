namespace Parley.Domain.Aggregates.ConversationAgg.Enums;

/// <summary>
/// Represents the role of a user within a group conversation.
/// </summary>
public enum GroupRole
{
    /// <summary>
    /// Regular member with limited permissions.
    /// </summary>
    Member = 0,

    /// <summary>
    /// Administrator with elevated permissions.
    /// </summary>
    Admin = 1
}

