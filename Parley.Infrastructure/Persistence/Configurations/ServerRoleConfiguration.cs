using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parley.Domain.Aggregates.ServerAgg.Entities;
using Parley.Domain.Aggregates.ServerAgg.Enums;

namespace Parley.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ServerRole entity.
/// Uses bitwise flags for permissions stored as ulong.
/// </summary>
public class ServerRoleConfiguration : IEntityTypeConfiguration<ServerRole>
{
    public void Configure(EntityTypeBuilder<ServerRole> builder)
    {
        // Table name
        builder.ToTable("server_roles");

        // Primary Key
        builder.HasKey(sr => sr.Id);

        // Properties
        builder.Property(sr => sr.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(sr => sr.ServerId)
            .HasColumnName("server_id")
            .IsRequired();

        builder.Property(sr => sr.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        // CRITICAL: Store permissions as ulong (bitwise flags)
        builder.Property(sr => sr.Permissions)
            .HasColumnName("permissions")
            .HasConversion<ulong>()
            .IsRequired();

        builder.Property(sr => sr.Color)
            .HasColumnName("color")
            .HasMaxLength(7)
            .IsRequired(false);

        builder.Property(sr => sr.Position)
            .HasColumnName("position")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(sr => sr.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(sr => sr.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        // Indexes
        builder.HasIndex(sr => new { sr.ServerId, sr.Name })
            .IsUnique()
            .HasDatabaseName("ix_server_roles_server_name");

        builder.HasIndex(sr => new { sr.ServerId, sr.Position })
            .HasDatabaseName("ix_server_roles_server_position");
    }
}

