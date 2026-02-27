using MediatR;
using Parley.Application._Shared.DTOs;
using Parley.Domain.Aggregates.MessageAgg.ValueObjects;

namespace Parley.Application.Features.Messages.Commands;

/// <summary>
/// Command to send a new message to a conversation.
/// Uses MediatR IRequest pattern with BaseResponse result.
/// </summary>
public class SendMessageCommand : IRequest<BaseResponse<SendMessageResponse>>
{
    /// <summary>
    /// The ID of the conversation to send the message to.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The ID of the user sending the message.
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// The text content of the message.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Optional URLs of attachments.
    /// </summary>
    public IReadOnlyList<string>? AttachmentUrls { get; set; }

    /// <summary>
    /// Optional embedded data (JSON string).
    /// </summary>
    public string? EmbedJsonData { get; set; }

    /// <summary>
    /// Optional ID of the message being replied to.
    /// </summary>
    public long? RepliedToMessageId { get; set; }

    /// <summary>
    /// List of user IDs to mention in this message.
    /// </summary>
    public IReadOnlyList<Guid>? MentionedUserIds { get; set; }
}

/// <summary>
/// Response DTO for SendMessageCommand.
/// </summary>
public class SendMessageResponse
{
    /// <summary>
    /// The generated Snowflake ID of the new message.
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// The timestamp when the message was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The ID of the conversation the message was sent to.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The ID of the user who sent the message.
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// The message text content.
    /// </summary>
    public string Text { get; set; } = string.Empty;
}

