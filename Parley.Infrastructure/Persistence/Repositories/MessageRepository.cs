using Parley.Domain.Aggregates.MessageAgg;
using Parley.Domain.Aggregates.MessageAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Message aggregate.
/// Focused on write operations. Queries use IContext directly.
/// Note: Message uses Snowflake ID (long) as the primary key for chronological ordering.
/// </summary>
public sealed class MessageRepository : RepositoryBase<Message, long>, IMessageRepository
{
    public MessageRepository(ParleyDbContext context) : base(context)
    {
    }
}


