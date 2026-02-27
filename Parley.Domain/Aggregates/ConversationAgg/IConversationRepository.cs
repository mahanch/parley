using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.ConversationAgg;

/// <summary>
/// Repository interface for Conversation aggregate operations.
/// </summary>
public interface IConversationRepository : IRepository<Conversation, Guid>
{
    /// <summary>
    /// Gets all conversations for a specific user (includes DMs, Groups, and Server channels they have access to).
    /// </summary>
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a direct message conversation between two users.
    /// </summary>
    Task<Conversation?> GetDirectConversationAsync(Guid userId1, Guid userId2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all channel conversations for a specific server.
    /// </summary>
    Task<IEnumerable<Conversation>> GetServerConversationsAsync(Guid serverId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a conversation with its participants loaded.
    /// </summary>
    Task<Conversation?> GetWithParticipantsAsync(Guid id, CancellationToken cancellationToken = default);
}

