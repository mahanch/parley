using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.MessageAgg;
using Parley.Domain.Aggregates.MessageAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Message aggregate.
/// Note: Message uses Snowflake ID (long) as the primary key for chronological ordering.
/// </summary>
public sealed class MessageRepository : RepositoryBase<Message, long>, IMessageRepository
{
    public MessageRepository(ParleyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Message>> GetConversationMessagesAsync(
        Guid conversationId, 
        int pageSize = 50, 
        long? beforeMessageId = null, 
        CancellationToken cancellationToken = default)
    {
        var query = Context.Messages
            .Where(m => m.ConversationId == conversationId);

        // Cursor-based pagination: get messages before the specified message ID
        if (beforeMessageId.HasValue)
        {
            query = query.Where(m => m.Id < beforeMessageId.Value);
        }

        // Order by ID descending (newest first) for efficient pagination
        return await query
            .OrderByDescending(m => m.Id)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Message?> GetLatestMessageAsync(
        Guid conversationId, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(
        Guid conversationId, 
        long? lastReadMessageId, 
        CancellationToken cancellationToken = default)
    {
        var query = Context.Messages
            .Where(m => m.ConversationId == conversationId);

        // If there's a watermark, count messages after it
        if (lastReadMessageId.HasValue)
        {
            query = query.Where(m => m.Id > lastReadMessageId.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetMessagesMentioningUserAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        // This will use JSONB array contains operator in PostgreSQL
        return await Context.Messages
            .Where(m => m.MentionedUserIds.Contains(userId))
            .OrderByDescending(m => m.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Message?> GetWithParentAsync(
        long id, 
        CancellationToken cancellationToken = default)
    {
        var message = await Context.Messages
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        // If this is a reply, load the parent message separately
        if (message?.RepliedToMessageId.HasValue == true)
        {
            // Parent message can be accessed via RepliedToMessageId if needed
            // You can manually load it in the application layer if required
        }

        return message;
    }
}


