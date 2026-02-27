using Parley.Domain.Aggregates.MessageAgg.Entities;
using Parley.Domain._Shared;

namespace Parley.Domain.Aggregates.MessageAgg;

/// <summary>
/// Repository interface for Message aggregate operations.
/// Focused on write operations only (Commands).
/// Query operations use IContext directly (Queries).
/// Note: Message uses Snowflake ID (long) as the primary key.
/// </summary>
public interface IMessageRepository : IRepository<Message, long>
{
    // Repository is now focused only on aggregate-root operations
    // Query methods moved to IContext for read efficiency
}

