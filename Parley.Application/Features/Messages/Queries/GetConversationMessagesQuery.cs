using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application.Features.Messages.Queries;

/// <summary>
/// Query to retrieve messages from a conversation with cursor-based pagination.
/// Uses Snowflake ID for efficient chronological ordering.
/// </summary>
public class GetConversationMessagesQuery : IRequest<BaseResponse<GetConversationMessagesResponse>>
{
    /// <summary>
    /// The conversation ID to retrieve messages from.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// Optional cursor: get messages before this message ID.
    /// Used for pagination (load older messages).
    /// </summary>
    public long? BeforeMessageId { get; set; }

    /// <summary>
    /// Number of messages to retrieve per page (default: 50).
    /// </summary>
    public int PageSize { get; set; } = 50;

    /// <summary>
    /// The user ID requesting the messages (for permission checks).
    /// </summary>
    public Guid RequestingUserId { get; set; }
}

/// <summary>
/// Response DTO for GetConversationMessagesQuery.
/// </summary>
public class GetConversationMessagesResponse
{
    /// <summary>
    /// List of messages retrieved.
    /// </summary>
    public IReadOnlyList<ConversationMessageDto> Messages { get; set; } = new List<ConversationMessageDto>();

    /// <summary>
    /// Total count of messages in the conversation.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// The ID of the oldest message in this response (for pagination).
    /// Use this as BeforeMessageId in the next query to load older messages.
    /// </summary>
    public long? OldestMessageId { get; set; }

    /// <summary>
    /// Indicates if there are more messages to load.
    /// </summary>
    public bool HasMoreMessages { get; set; }
}

/// <summary>
/// DTO representing a single message in a conversation.
/// </summary>
public class ConversationMessageDto
{
    /// <summary>
    /// The Snowflake ID of the message.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The ID of the user who sent this message.
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// The text content of the message.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// URLs of attached files.
    /// </summary>
    public IReadOnlyList<string> AttachmentUrls { get; set; } = new List<string>();

    /// <summary>
    /// Embedded data (if any).
    /// </summary>
    public string? EmbedJsonData { get; set; }

    /// <summary>
    /// Type of message (Default, Reply, System).
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// If this is a reply, the ID of the message being replied to.
    /// </summary>
    public long? RepliedToMessageId { get; set; }

    /// <summary>
    /// IDs of users mentioned in this message.
    /// </summary>
    public IReadOnlyList<Guid> MentionedUserIds { get; set; } = new List<Guid>();

    /// <summary>
    /// Timestamp when the message was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the message was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Whether this message has been deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}

