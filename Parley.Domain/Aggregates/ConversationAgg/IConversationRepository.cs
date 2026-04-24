﻿using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.ConversationAgg;

/// <summary>
/// Repository interface for Conversation aggregate operations.
/// Focused on write operations (Commands) and existence checks for business logic.
/// Query operations that return DTOs use Query Services.
/// </summary>
public interface IConversationRepository : IRepository<Conversation, Guid>
{
    /// <summary>
    /// Gets a conversation participant for update operations.
    /// This returns a tracked entity that can be modified.
    /// </summary>
    Task<ConversationParticipant?> GetParticipantAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if a user is a participant in a conversation.
    /// Used for permission validation in command handlers.
    /// </summary>
    Task<bool> IsUserParticipantAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds an existing direct conversation between two users.
    /// Returns null if no direct conversation exists.
    /// </summary>
    Task<Guid?> FindDirectConversationAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken = default);
}
