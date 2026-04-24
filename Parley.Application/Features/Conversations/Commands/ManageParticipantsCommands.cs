using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application.Features.Conversations.Commands;

/// <summary>
/// Command to add a participant to a group conversation.
/// </summary>
public class AddParticipantCommand : IRequest<BaseResponse>
{
    /// <summary>
    /// The conversation ID.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The user ID to add.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The user ID performing the action (must be admin).
    /// </summary>
    public Guid RequestedByUserId { get; set; }
}

/// <summary>
/// Command to remove a participant from a group conversation.
/// </summary>
public class RemoveParticipantCommand : IRequest<BaseResponse>
{
    /// <summary>
    /// The conversation ID.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The user ID to remove.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The user ID performing the action (must be admin or the user themselves).
    /// </summary>
    public Guid RequestedByUserId { get; set; }
}

/// <summary>
/// Command to leave a conversation.
/// </summary>
public class LeaveConversationCommand : IRequest<BaseResponse>
{
    /// <summary>
    /// The conversation ID.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The user ID leaving the conversation.
    /// </summary>
    public Guid UserId { get; set; }
}
