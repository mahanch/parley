using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.MessageAgg.Entities;
using Parley.Domain.Aggregates.ServerAgg.Entities;

namespace Parley.Application.Contracts.Interfaces.Data;

/// <summary>
/// Read-optimized context interface for queries.
/// Provides direct access to DbSets for efficient data retrieval without repository overhead.
/// Used exclusively in Query handlers, NOT in Command handlers.
/// </summary>
public interface IContext
{
    /// <summary>
    /// DbSet for Conversations.
    /// </summary>
    IQueryable<Conversation> Conversations { get; }

    /// <summary>
    /// DbSet for ConversationParticipants.
    /// </summary>
    IQueryable<ConversationParticipant> ConversationParticipants { get; }

    /// <summary>
    /// DbSet for Messages.
    /// </summary>
    IQueryable<Message> Messages { get; }

    /// <summary>
    /// DbSet for Servers.
    /// </summary>
    IQueryable<Server> Servers { get; }

    /// <summary>
    /// DbSet for ServerRoles.
    /// </summary>
    IQueryable<ServerRole> ServerRoles { get; }

    /// <summary>
    /// DbSet for ServerMembers.
    /// </summary>
    IQueryable<ServerMember> ServerMembers { get; }
}

