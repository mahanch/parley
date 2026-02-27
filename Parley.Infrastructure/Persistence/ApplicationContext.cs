using Parley.Application.Contracts.Interfaces.Data;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.MessageAgg.Entities;
using Parley.Domain.Aggregates.ServerAgg.Entities;

namespace Parley.Infrastructure.Persistence;

/// <summary>
/// Implementation of IContext wrapping ParleyDbContext.
/// Provides read-optimized access to entities for queries.
/// </summary>
public class ApplicationContext : IContext
{
    private readonly ParleyDbContext _dbContext;

    public ApplicationContext(ParleyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IQueryable<Conversation> Conversations => _dbContext.Conversations;

    public IQueryable<ConversationParticipant> ConversationParticipants => _dbContext.ConversationParticipants;

    public IQueryable<Message> Messages => _dbContext.Messages;

    public IQueryable<Server> Servers => _dbContext.Servers;

    public IQueryable<ServerRole> ServerRoles => _dbContext.ServerRoles;

    public IQueryable<ServerMember> ServerMembers => _dbContext.ServerMembers;
}

