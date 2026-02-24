using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg.Enums;

namespace Parley.Domain.Aggregates.ConversationAgg.Entities;

/// <summary>
/// Represents a participant in a conversation.
/// This is an Entity within the Conversation aggregate.
/// </summary>
public class ConversationParticipant : EntityBase<Guid>
{
    /// <summary>
    /// The ID of the conversation this participant belongs to.
    /// </summary>
    public Guid ConversationId { get; private set; }

    /// <summary>
    /// The ID of the user who is a participant.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// The role of this participant in the conversation.
    /// </summary>
    public GroupRole Role { get; private set; }

    /// <summary>
    /// The ID of the last message read by this participant.
    /// Used to calculate unread messages (Watermark pattern).
    /// </summary>
    public long? LastReadMessageId { get; private set; }

    /// <summary>
    /// The timestamp when this participant joined the conversation.
    /// </summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private ConversationParticipant()
    {
    }

    /// <summary>
    /// Creates a new ConversationParticipant.
    /// </summary>
    public ConversationParticipant(Guid conversationId, Guid userId, GroupRole role = GroupRole.Member)
    {
        if (conversationId == Guid.Empty)
            throw new ArgumentException("Conversation ID cannot be empty.", nameof(conversationId));

        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        ConversationId = conversationId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the watermark (last read message ID) for this participant.
    /// </summary>
    public void UpdateWatermark(long messageId)
    {
        if (messageId <= 0)
            throw new ArgumentException("Message ID must be greater than 0.", nameof(messageId));

        // Only update if the new message ID is more recent
        if (LastReadMessageId == null || messageId > LastReadMessageId)
        {
            LastReadMessageId = messageId;
        }
    }

    /// <summary>
    /// Updates the role of this participant.
    /// </summary>
    public void UpdateRole(GroupRole newRole)
    {
        Role = newRole;
    }
}

