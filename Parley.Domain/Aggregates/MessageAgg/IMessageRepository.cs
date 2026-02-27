using Parley.Domain.Aggregates.MessageAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.MessageAgg;

/// <summary>
/// Repository interface for Message aggregate operations.
/// Note: Message uses Snowflake ID (long) as the primary key.
/// </summary>
public interface IMessageRepository : IRepository<Message, long>
{
    /// <summary>
    /// Gets messages in a conversation with pagination support.
    /// Uses cursor-based pagination with beforeMessageId for efficient scrolling.
    /// </summary>
    Task<IEnumerable<Message>> GetConversationMessagesAsync(
        Guid conversationId, 
        int pageSize = 50, 
        long? beforeMessageId = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest message in a conversation.
    /// </summary>
    Task<Message?> GetLatestMessageAsync(Guid conversationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets unread messages count for a user in a conversation after a specific message ID (watermark).
    /// </summary>
    Task<int> GetUnreadCountAsync(Guid conversationId, long? lastReadMessageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets messages that mention a specific user.
    /// </summary>
    Task<IEnumerable<Message>> GetMessagesMentioningUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a message by ID with its parent message loaded (for replies).
    /// </summary>
    Task<Message?> GetWithParentAsync(long id, CancellationToken cancellationToken = default);
}

