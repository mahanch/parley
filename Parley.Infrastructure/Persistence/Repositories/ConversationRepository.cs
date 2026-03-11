using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.ConversationAgg.Entities;

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
}

