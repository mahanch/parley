using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.MessageAgg;
using Parley.Domain.Aggregates.ServerAgg;
using Parley.Infrastructure.Persistence;
using Parley.Infrastructure.Persistence.Repositories;
using Parley.Infrastructure._Shared.Services;
using StackExchange.Redis;

namespace Parley.Infrastructure._Shared.Extensions;

/// <summary>
/// Dependency injection extensions for the Infrastructure layer.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the DI container.
    /// Includes DbContext, repositories, Snowflake ID generator, and Redis cache.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString,
        string redisConnectionString = "localhost:6379",
        int? snowflakeGeneratorId = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(connectionString);
        ArgumentNullException.ThrowIfNull(redisConnectionString);

        // Add PostgreSQL DbContext
        services.AddDbContext<ParleyDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory");
            });

            // Enable detailed error messages in development
            // options.EnableSensitiveDataLogging();
        });

        // Register repositories
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IServerRepository, ServerRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Snowflake ID Generator
        var generatorId = snowflakeGeneratorId ?? 0;
        services.AddSingleton<ISnowflakeIdGenerator>(
            new SnowflakeIdGenerator(generatorId));

        // Register Redis Cache
        var redisOptions = ConfigurationOptions.Parse(redisConnectionString);
        var redisConnection = ConnectionMultiplexer.Connect(redisOptions);
        services.AddSingleton(redisConnection);
        services.AddSingleton<IRedisCache, RedisCache>();

        return services;
    }

    /// <summary>
    /// Applies pending database migrations during application startup.
    /// Call this after building the host.
    /// </summary>
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ParleyDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}


