using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg.Enums;
using Parley.Domain.Aggregates.ConversationAgg.Exceptions;

namespace Parley.Domain.Aggregates.ConversationAgg.Entities;

/// <summary>
/// Represents a conversation (Aggregate Root).
/// This can be a direct message, group chat, or server channel.
/// </summary>
public class Conversation : AggregateRoot<Guid>
{
    /// <summary>
    /// The server ID if this conversation is a server channel.
    /// Null for direct messages and group chats.
    /// </summary>
    public Guid? ServerId { get; private set; }

    /// <summary>
    /// The type of this conversation.
    /// </summary>
    public ConversationType Type { get; private set; }

    /// <summary>
    /// The name of this conversation (used for group chats and server channels).
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Private list of participants in this conversation.
    /// </summary>
    private readonly List<ConversationParticipant> _participants = new();

    /// <summary>
    /// Gets the read-only list of participants.
    /// </summary>
    public IReadOnlyList<ConversationParticipant> Participants => _participants.AsReadOnly();

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private Conversation()
    {
    }

    /// <summary>
    /// Creates a new Conversation.
    /// </summary>
    public Conversation(ConversationType type, Guid? serverId = null, string? name = null)
    {
        if (type == ConversationType.ServerChannel && serverId == null)
            throw new ConversationException("Server channels must have a ServerId.");

        if (type == ConversationType.ServerChannel && string.IsNullOrWhiteSpace(name))
            throw new ConversationException("Server channels must have a name.");

        Id = Guid.NewGuid();
        Type = type;
        ServerId = serverId;
        Name = name;
    }

    /// <summary>
    /// Adds a participant to this conversation.
    /// RULE: Server channels cannot have explicit participants (they use implicit role-based access).
    /// </summary>
    public void AddParticipant(Guid userId, GroupRole role = GroupRole.Member)
    {
        if (Type == ConversationType.ServerChannel)
            throw new ConversationException("Server channels use implicit role-based access. Cannot add explicit participants.");

        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        if (_participants.Any(p => p.UserId == userId))
            return; // Participant already exists

        var participant = new ConversationParticipant(Id, userId, role);
        _participants.Add(participant);
        MarkAsModified();
    }

    /// <summary>
    /// Removes a participant from this conversation.
    /// </summary>
    public void RemoveParticipant(Guid userId)
    {
        if (Type == ConversationType.ServerChannel)
            throw new ConversationException("Server channels use implicit role-based access. Cannot remove explicit participants.");

        var participant = _participants.FirstOrDefault(p => p.UserId == userId);
        if (participant != null)
        {
            _participants.Remove(participant);
            MarkAsModified();
        }
    }

    /// <summary>
    /// Updates the conversation name.
    /// </summary>
    public void UpdateName(string? name)
    {
        if (Type == ConversationType.Direct)
            throw new ConversationException("Direct messages cannot have custom names.");

        Name = name;
        MarkAsModified();
    }

    /// <summary>
    /// Promotes a participant to admin.
    /// </summary>
    public void PromoteParticipant(Guid userId)
    {
        if (Type != ConversationType.Group)
            throw new ConversationException("Only group conversations can have admin participants.");

        var participant = _participants.FirstOrDefault(p => p.UserId == userId);
        if (participant == null)
            throw new ConversationException($"User {userId} is not a participant in this conversation.");

        participant.UpdateRole(GroupRole.Admin);
        MarkAsModified();
    }

    /// <summary>
    /// Demotes a participant to member.
    /// </summary>
    public void DemoteParticipant(Guid userId)
    {
        if (Type != ConversationType.Group)
            throw new ConversationException("Only group conversations can have admin participants.");

        var participant = _participants.FirstOrDefault(p => p.UserId == userId);
        if (participant == null)
            throw new ConversationException($"User {userId} is not a participant in this conversation.");

        participant.UpdateRole(GroupRole.Member);
        MarkAsModified();
    }
}

