using Microsoft.EntityFrameworkCore;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.MessageAgg.Entities;
using Parley.Domain.Aggregates.MessageAgg.ValueObjects;
using Parley.Domain.Aggregates.ServerAgg.Entities;
using System.Reflection;

namespace Parley.Infrastructure.Persistence;

/// <summary>
/// Main DbContext for the Parley Discord-clone application.
/// Handles all data persistence for conversations, messages, and servers.
/// Uses EF Core with PostgreSQL and JSONB features for flexible data storage.
/// </summary>
public class ParleyDbContext : DbContext
{
    /// <summary>
    /// DbSet for Conversations (Aggregate Root).
    /// </summary>
    public DbSet<Conversation> Conversations { get; set; }

    /// <summary>
    /// DbSet for ConversationParticipants (Entity).
    /// </summary>
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; }

    /// <summary>
    /// DbSet for Messages (Aggregate Root with Snowflake ID).
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// DbSet for Servers (Aggregate Root).
    /// </summary>
    public DbSet<Server> Servers { get; set; }

    /// <summary>
    /// DbSet for ServerRoles (Entity).
    /// </summary>
    public DbSet<ServerRole> ServerRoles { get; set; }

    /// <summary>
    /// DbSet for ServerMembers (Entity).
    /// </summary>
    public DbSet<ServerMember> ServerMembers { get; set; }

    /// <summary>
    /// Creates a new instance of ParleyDbContext.
    /// </summary>
    public ParleyDbContext(DbContextOptions<ParleyDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the database model and entity mappings.
    /// Uses IEntityTypeConfiguration classes from the Configurations folder.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    

    /// <summary>
    /// Overrides SaveChanges to update timestamps.
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Overrides SaveChangesAsync to update timestamps.
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates CreatedAt and UpdatedAt timestamps for tracked entities.
    /// </summary>
    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().GetProperty("CreatedAt") != null);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}

