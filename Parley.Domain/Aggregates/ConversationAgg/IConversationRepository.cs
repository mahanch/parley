﻿using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.ConversationAgg;

/// <summary>
/// Repository interface for Conversation aggregate operations.
/// Focused on write operations only (Commands).
/// Query operations use IContext directly (Queries).
/// </summary>
public interface IConversationRepository : IRepository<Conversation, Guid>
{
    /// <summary>
    /// Gets a conversation participant for update operations.
    /// This returns a tracked entity that can be modified.
    /// </summary>
    Task<ConversationParticipant?> GetParticipantAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);
}

