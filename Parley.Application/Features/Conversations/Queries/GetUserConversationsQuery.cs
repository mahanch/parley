using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application.Features.Conversations.Queries;

/// <summary>
/// Query to get all conversations for a user.
/// </summary>
public class GetUserConversationsQuery : IRequest<BaseResponse<GetUserConversationsResponse>>
{
    /// <summary>
    /// The user ID to get conversations for.
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Response for GetUserConversationsQuery.
/// </summary>
public class GetUserConversationsResponse
{
    /// <summary>
    /// List of user's conversations.
    /// </summary>
    public IReadOnlyList<ConversationSummaryDto> Conversations { get; set; } = new List<ConversationSummaryDto>();
}

/// <summary>
/// DTO for conversation summary.
/// </summary>
public class ConversationSummaryDto
{
    /// <summary>
    /// Conversation ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Conversation type (Direct or Group).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Conversation name (null for direct).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Last message in the conversation (if any).
    /// </summary>
    public MessageSummaryDto? LastMessage { get; set; }

    /// <summary>
    /// Number of unread messages.
    /// </summary>
    public int UnreadCount { get; set; }

    /// <summary>
    /// Timestamp of last activity.
    /// </summary>
    public DateTime? LastActivityAt { get; set; }
}

/// <summary>
/// DTO for message summary.
/// </summary>
public class MessageSummaryDto
{
    /// <summary>
    /// Message ID.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Sender's display name.
    /// </summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// Message text preview.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// When the message was sent.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
