using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.ConversationAgg.Enums;

namespace Parley.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ConversationParticipant entity.
/// </summary>
public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
{
    public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        // Table name
        builder.ToTable("conversation_participants");

        // Primary Key
        builder.HasKey(cp => cp.Id);

        // Properties
        builder.Property(cp => cp.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(cp => cp.ConversationId)
            .HasColumnName("conversation_id")
            .IsRequired();

        builder.Property(cp => cp.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(cp => cp.Role)
            .HasColumnName("role")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(cp => cp.LastReadMessageId)
            .HasColumnName("last_read_message_id")
            .IsRequired(false);

        builder.Property(cp => cp.JoinedAt)
            .HasColumnName("joined_at")
            .IsRequired();

        builder.Property(cp => cp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(cp => cp.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        // Indexes
        builder.HasIndex(cp => new { cp.ConversationId, cp.UserId })
            .IsUnique()
            .HasDatabaseName("ix_conversation_participants_conversation_user");

        builder.HasIndex(cp => cp.UserId)
            .HasDatabaseName("ix_conversation_participants_user_id");

        builder.HasIndex(cp => cp.LastReadMessageId)
            .HasDatabaseName("ix_conversation_participants_last_read_message_id");
    }
}

