using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parley.Domain.Aggregates.ServerAgg.Entities;
using Parley.Domain.Aggregates.ServerAgg.Enums;
using System.Text.Json;

namespace Parley.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Server aggregate root.
/// </summary>
public class ServerConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        // Table name
        builder.ToTable("servers");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(s => s.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property(s => s.IconUrl)
            .HasColumnName("icon_url")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(s => s.IsPublic)
            .HasColumnName("is_public")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired(false);

        // Relationships
        builder.HasMany(s => s.Members)
            .WithOne()
            .HasForeignKey(sm => sm.ServerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Roles)
            .WithOne()
            .HasForeignKey(sr => sr.ServerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.OwnerId)
            .HasDatabaseName("ix_servers_owner_id");

        builder.HasIndex(s => s.Name)
            .HasDatabaseName("ix_servers_name");

        builder.HasIndex(s => s.CreatedAt)
            .HasDatabaseName("ix_servers_created_at");

        // Navigation properties
        builder.Navigation(s => s.Members)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(s => s.Roles)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

