using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Infrastructure.Persistence.Configurations;

public class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x=>x.FirstName)
            .HasColumnName("first_name").IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.LastName).
            HasColumnName("last_name").IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.Email)
            .HasColumnName("email").IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.Password)
            .HasColumnName("password").IsRequired().HasMaxLength(50);
        
        builder.Property(x=>x.Username)
            .HasColumnName("username").IsRequired().HasMaxLength(50);
        
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Username).IsUnique();
        
    }
}