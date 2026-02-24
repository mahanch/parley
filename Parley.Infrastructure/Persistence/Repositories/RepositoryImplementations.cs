using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.ConversationAgg.Enums;

namespace Parley.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for Conversation aggregate operations.
/// </summary>
public class ConversationRepository : IConversationRepository
{
    private readonly ParleyDbContext _context;

    public ConversationRepository(ParleyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Conversation?> GetByIdAsync(Guid id)
    {
        return await _context.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId)
    {
        return await _context.Conversations
            .Include(c => c.Participants)
            .Where(c => c.Type != ConversationType.ServerChannel &&
                        c.Participants.Any(p => p.UserId == userId))
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Conversation?> GetDirectConversationAsync(Guid userId1, Guid userId2)
    {
        return await _context.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c =>
                c.Type == ConversationType.Direct &&
                c.Participants.Any(p => p.UserId == userId1) &&
                c.Participants.Any(p => p.UserId == userId2));
    }

    public async Task AddAsync(Conversation conversation)
    {
        ArgumentNullException.ThrowIfNull(conversation);
        await _context.Conversations.AddAsync(conversation);
    }

    public Task UpdateAsync(Conversation conversation)
    {
        ArgumentNullException.ThrowIfNull(conversation);
        _context.Conversations.Update(conversation);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var conversation = await GetByIdAsync(id);
        if (conversation != null)
        {
            _context.Conversations.Remove(conversation);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

/// <summary>
/// Repository for Message aggregate operations.
/// </summary>
public class MessageRepository : IMessageRepository
{
    private readonly ParleyDbContext _context;

    public MessageRepository(ParleyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Domain.Aggregates.MessageAgg.Entities.Message?> GetByIdAsync(long id)
    {
        return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Domain.Aggregates.MessageAgg.Entities.Message>> GetConversationMessagesAsync(
        Guid conversationId, int pageSize = 50, long? beforeMessageId = null)
    {
        var query = _context.Messages
            .Where(m => m.ConversationId == conversationId && !m.IsDeleted);

        if (beforeMessageId.HasValue)
        {
            query = query.Where(m => m.Id < beforeMessageId.Value);
        }

        return await query
            .OrderByDescending(m => m.Id)
            .Take(pageSize)
            .OrderBy(m => m.Id)
            .ToListAsync();
    }

    public async Task<Domain.Aggregates.MessageAgg.Entities.Message?> GetLatestMessageAsync(Guid conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
            .OrderByDescending(m => m.Id)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Domain.Aggregates.MessageAgg.Entities.Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        await _context.Messages.AddAsync(message);
    }

    public Task UpdateAsync(Domain.Aggregates.MessageAgg.Entities.Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        _context.Messages.Update(message);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id)
    {
        var message = await GetByIdAsync(id);
        if (message != null)
        {
            _context.Messages.Remove(message);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

/// <summary>
/// Repository for Server aggregate operations.
/// </summary>
public class ServerRepository : IServerRepository
{
    private readonly ParleyDbContext _context;

    public ServerRepository(ParleyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Domain.Aggregates.ServerAgg.Entities.Server?> GetByIdAsync(Guid id)
    {
        return await _context.Servers
            .Include(s => s.Members)
            .Include(s => s.Roles)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Domain.Aggregates.ServerAgg.Entities.Server>> GetUserServersAsync(Guid userId)
    {
        return await _context.Servers
            .Include(s => s.Members)
            .Include(s => s.Roles)
            .Where(s => s.Members.Any(m => m.UserId == userId))
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Aggregates.ServerAgg.Entities.Server>> GetPublicServersAsync(int pageSize = 20, int pageNumber = 1)
    {
        return await _context.Servers
            .Include(s => s.Members)
            .Include(s => s.Roles)
            .Where(s => s.IsPublic)
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Aggregates.ServerAgg.Entities.Server server)
    {
        ArgumentNullException.ThrowIfNull(server);
        await _context.Servers.AddAsync(server);
    }

    public Task UpdateAsync(Domain.Aggregates.ServerAgg.Entities.Server server)
    {
        ArgumentNullException.ThrowIfNull(server);
        _context.Servers.Update(server);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var server = await GetByIdAsync(id);
        if (server != null)
        {
            _context.Servers.Remove(server);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

