using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application.Features.Conversations.Commands;

/// <summary>
/// Command to create a new direct conversation between two users.
/// </summary>
public class CreateDirectConversationCommand : IRequest<BaseResponse<CreateConversationResponse>>
{
    /// <summary>
    /// The ID of the first user.
    /// </summary>
    public Guid UserId1 { get; set; }

    /// <summary>
    /// The ID of the second user.
    /// </summary>
    public Guid UserId2 { get; set; }
}

/// <summary>
/// Command to create a new group conversation.
/// </summary>
public class CreateGroupConversationCommand : IRequest<BaseResponse<CreateConversationResponse>>
{
    /// <summary>
    /// The name of the group conversation.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the user creating the group.
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// List of user IDs to add to the group initially.
    /// </summary>
    public IReadOnlyList<Guid> ParticipantIds { get; set; } = new List<Guid>();
}

/// <summary>
/// Response DTO for conversation creation commands.
/// </summary>
public class CreateConversationResponse
{
    /// <summary>
    /// The ID of the created conversation.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The type of the conversation.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The name of the conversation (null for direct).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The timestamp when the conversation was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
