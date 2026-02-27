using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.ConversationAgg.Enums;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Conversation aggregate.
/// </summary>
public sealed class ConversationRepository : RepositoryBase<Conversation, Guid>, IConversationRepository
{
    public ConversationRepository(ParleyDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        // Get all conversations where the user is a participant
        return await Context.Conversations
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Conversation?> GetDirectConversationAsync(
        Guid userId1, 
        Guid userId2, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Conversations
            .Include(c => c.Participants)
            .Where(c => c.Type == ConversationType.Direct &&
                        c.Participants.Any(p => p.UserId == userId1) &&
                        c.Participants.Any(p => p.UserId == userId2))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Conversation>> GetServerConversationsAsync(
        Guid serverId, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Conversations
            .Where(c => c.ServerId == serverId && c.Type == ConversationType.ServerChannel)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Conversation?> GetWithParticipantsAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await Context.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}

