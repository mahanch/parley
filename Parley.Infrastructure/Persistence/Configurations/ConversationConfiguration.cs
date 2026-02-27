using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.ConversationAgg.Enums;

namespace Parley.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Conversation aggregate root.
/// </summary>
public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        // Table name
        builder.ToTable("conversations");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(c => c.ServerId)
            .HasColumnName("server_id")
            .IsRequired(false);

        builder.Property(c => c.Type)
            .HasColumnName("type")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        // Relationships
        builder.HasMany(c => c.Participants)
            .WithOne()
            .HasForeignKey(cp => cp.ConversationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.Type)
            .HasDatabaseName("ix_conversations_type");

        builder.HasIndex(c => c.ServerId)
            .HasDatabaseName("ix_conversations_server_id");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("ix_conversations_created_at");

        // Ignore navigation property (if using _participants backing field)
        builder.Navigation(c => c.Participants)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

