using Parley.Domain.Aggregates.ConversationAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository interface for Conversation aggregate operations.
/// </summary>
public interface IConversationRepository
{
    /// <summary>
    /// Gets a conversation by ID.
    /// </summary>
    Task<Conversation?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all conversations for a specific user.
    /// </summary>
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId);

    /// <summary>
    /// Gets a direct message conversation between two users.
    /// </summary>
    Task<Conversation?> GetDirectConversationAsync(Guid userId1, Guid userId2);

    /// <summary>
    /// Adds a new conversation.
    /// </summary>
    Task AddAsync(Conversation conversation);

    /// <summary>
    /// Updates an existing conversation.
    /// </summary>
    Task UpdateAsync(Conversation conversation);

    /// <summary>
    /// Deletes a conversation.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}

/// <summary>
/// Repository interface for Message aggregate operations.
/// </summary>
public interface IMessageRepository
{
    /// <summary>
    /// Gets a message by its Snowflake ID.
    /// </summary>
    Task<Domain.Aggregates.MessageAgg.Entities.Message?> GetByIdAsync(long id);

    /// <summary>
    /// Gets messages in a conversation with pagination.
    /// </summary>
    Task<IEnumerable<Domain.Aggregates.MessageAgg.Entities.Message>> GetConversationMessagesAsync(
        Guid conversationId, int pageSize = 50, long? beforeMessageId = null);

    /// <summary>
    /// Gets the latest message in a conversation.
    /// </summary>
    Task<Domain.Aggregates.MessageAgg.Entities.Message?> GetLatestMessageAsync(Guid conversationId);

    /// <summary>
    /// Adds a new message.
    /// </summary>
    Task AddAsync(Domain.Aggregates.MessageAgg.Entities.Message message);

    /// <summary>
    /// Updates an existing message.
    /// </summary>
    Task UpdateAsync(Domain.Aggregates.MessageAgg.Entities.Message message);

    /// <summary>
    /// Deletes a message.
    /// </summary>
    Task DeleteAsync(long id);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}

/// <summary>
/// Repository interface for Server aggregate operations.
/// </summary>
public interface IServerRepository
{
    /// <summary>
    /// Gets a server by ID.
    /// </summary>
    Task<Domain.Aggregates.ServerAgg.Entities.Server?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all servers a user is a member of.
    /// </summary>
    Task<IEnumerable<Domain.Aggregates.ServerAgg.Entities.Server>> GetUserServersAsync(Guid userId);

    /// <summary>
    /// Gets all public servers with pagination.
    /// </summary>
    Task<IEnumerable<Domain.Aggregates.ServerAgg.Entities.Server>> GetPublicServersAsync(int pageSize = 20, int pageNumber = 1);

    /// <summary>
    /// Adds a new server.
    /// </summary>
    Task AddAsync(Domain.Aggregates.ServerAgg.Entities.Server server);

    /// <summary>
    /// Updates an existing server.
    /// </summary>
    Task UpdateAsync(Domain.Aggregates.ServerAgg.Entities.Server server);

    /// <summary>
    /// Deletes a server.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}

