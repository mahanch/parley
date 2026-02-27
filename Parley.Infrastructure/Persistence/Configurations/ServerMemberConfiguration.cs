using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parley.Domain.Aggregates.ServerAgg.Entities;
using System.Text.Json;

namespace Parley.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ServerMember entity.
/// RoleIds are stored as JSONB array for flexibility.
/// </summary>
public class ServerMemberConfiguration : IEntityTypeConfiguration<ServerMember>
{
    public void Configure(EntityTypeBuilder<ServerMember> builder)
    {
        // Table name
        builder.ToTable("server_members");

        // Primary Key
        builder.HasKey(sm => sm.Id);

        // Properties
        builder.Property(sm => sm.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(sm => sm.ServerId)
            .HasColumnName("server_id")
            .IsRequired();

        builder.Property(sm => sm.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(sm => sm.Nickname)
            .HasColumnName("nickname")
            .HasMaxLength(32)
            .IsRequired(false);

        // JSONB Configuration for RoleIds (List<Guid>)
        builder.Property(sm => sm.RoleIds)
            .HasColumnName("role_ids")
            .HasColumnType("jsonb")
            .IsRequired()
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>()
            );

        builder.Property(sm => sm.JoinedAt)
            .HasColumnName("joined_at")
            .IsRequired();

        builder.Property(sm => sm.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sm => sm.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        // Indexes
        builder.HasIndex(sm => new { sm.ServerId, sm.UserId })
            .IsUnique()
            .HasDatabaseName("ix_server_members_server_user");

        builder.HasIndex(sm => sm.UserId)
            .HasDatabaseName("ix_server_members_user_id");

        builder.HasIndex(sm => sm.JoinedAt)
            .HasDatabaseName("ix_server_members_joined_at");
    }
}

