using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.ConversationAgg.Entities;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Conversation aggregate.
/// Focused on write operations. Queries use IContext directly.
/// </summary>
public sealed class ConversationRepository : RepositoryBase<Conversation, Guid>, IConversationRepository
{
    public ConversationRepository(ParleyDbContext context) : base(context)
    {
    }
}

