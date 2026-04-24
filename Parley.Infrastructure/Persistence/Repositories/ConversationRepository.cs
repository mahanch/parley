using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.ConversationAgg.Enums;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for Conversation aggregate.
/// Focused on write operations and existence checks for business logic.
/// Queries that return DTOs use Query Services.
/// </summary>
public sealed class ConversationRepository : RepositoryBase<Conversation, Guid>, IConversationRepository
{
    private readonly ParleyDbContext _context;
    public ConversationRepository(ParleyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ConversationParticipant?> GetParticipantAsync(
        Guid conversationId, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ConversationParticipants
            .FirstOrDefaultAsync(
                cp => cp.ConversationId == conversationId && cp.UserId == userId,
                cancellationToken);
    }

    public async Task<bool> IsUserParticipantAsync(
        Guid conversationId, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.ConversationParticipants
            .AnyAsync(
                cp => cp.ConversationId == conversationId && cp.UserId == userId,
                cancellationToken);
    }

    public async Task<Guid?> FindDirectConversationAsync(
        Guid userId1, 
        Guid userId2, 
        CancellationToken cancellationToken = default)
    {
        // Find conversations where both users are participants and type is Direct
        var conversationIds = await _context.ConversationParticipants
            .Where(cp => cp.UserId == userId1 || cp.UserId == userId2)
            .GroupBy(cp => cp.ConversationId)
            .Where(g => g.Count() == 2) // Exactly two participants
            .Select(g => g.Key)
            .ToListAsync(cancellationToken);

        // Check which of these are Direct conversations
        foreach (var conversationId in conversationIds)
        {
            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.Type == ConversationType.Direct, cancellationToken);
            if (conversation != null)
            {
                return conversation.Id;
            }
        }

        return null;
    }
}
