using Parley.Domain._Shared;
using Parley.Domain.Aggregates.MessageAgg.Enums;
using Parley.Domain.Aggregates.MessageAgg.Exceptions;
using Parley.Domain.Aggregates.MessageAgg.ValueObjects;

namespace Parley.Domain.Aggregates.MessageAgg.Entities;

/// <summary>
/// Represents a message in a conversation (Aggregate Root).
/// 
/// CRITICAL: The Id is a long (Snowflake ID) for chronological sorting and high-scale support.
/// </summary>
public class Message : AggregateRoot<long>
{

    /// <summary>
    /// The ID of the conversation this message belongs to.
    /// </summary>
    public Guid ConversationId { get; private set; }

    /// <summary>
    /// The ID of the user who sent this message.
    /// </summary>
    public Guid SenderId { get; private set; }

    /// <summary>
    /// The type of this message.
    /// </summary>
    public MessageType Type { get; private set; }

    /// <summary>
    /// The content of the message (text, attachments, embeds).
    /// Stored as JSONB in PostgreSQL.
    /// </summary>
    public MessageContent Content { get; private set; }

    /// <summary>
    /// If this is a reply, this contains the ID of the message being replied to.
    /// </summary>
    public long? RepliedToMessageId { get; private set; }

    /// <summary>
    /// List of user IDs mentioned in this message.
    /// </summary>
    public IReadOnlyList<Guid> MentionedUserIds { get; private set; }

    /// <summary>
    /// Version number for optimistic concurrency control.
    /// </summary>
    public uint Version { get; private set; }

    /// <summary>
    /// Indicates whether this message has been deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private Message()
    {
        MentionedUserIds = new List<Guid>();
        Content = null!;
    }

    /// <summary>
    /// Creates a new Message.
    /// </summary>
    public Message(
        long id,
        Guid conversationId,
        Guid senderId,
        MessageContent content,
        MessageType type = MessageType.Default,
        long? repliedToMessageId = null,
        IReadOnlyList<Guid>? mentionedUserIds = null)
    {
        if (id <= 0)
            throw new ArgumentException("Message ID must be a valid Snowflake ID.", nameof(id));

        if (conversationId == Guid.Empty)
            throw new ArgumentException("Conversation ID cannot be empty.", nameof(conversationId));

        if (senderId == Guid.Empty)
            throw new ArgumentException("Sender ID cannot be empty.", nameof(senderId));

        if (content is null)
            throw new ArgumentNullException(nameof(content), "Message content cannot be null.");

        Id = id;
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        Type = type;
        RepliedToMessageId = repliedToMessageId;
        MentionedUserIds = mentionedUserIds ?? new List<Guid>();
        Version = 1;
    }

    /// <summary>
    /// Updates the content of this message.
    /// </summary>
    public void UpdateContent(MessageContent newContent)
    {
        if (newContent is null)
            throw new ArgumentNullException(nameof(newContent), "Message content cannot be null.");

        if (IsDeleted)
            throw new MessageException("Cannot update a deleted message.");

        Content = newContent;
        Version++;
        MarkAsModified();
    }

    /// <summary>
    /// Marks this message as deleted.
    /// </summary>
    public void Delete()
    {
        if (IsDeleted)
            throw new MessageException("Message is already deleted.");

        IsDeleted = true;
        Version++;
        MarkAsModified();
    }

    /// <summary>
    /// Updates the list of mentioned user IDs.
    /// </summary>
    public void UpdateMentions(IReadOnlyList<Guid> mentionedUserIds)
    {
        if (mentionedUserIds is null)
            throw new ArgumentNullException(nameof(mentionedUserIds), "Mentioned user IDs cannot be null.");

        if (IsDeleted)
            throw new MessageException("Cannot update mentions on a deleted message.");

        MentionedUserIds = mentionedUserIds;
        MarkAsModified();
    }
}

