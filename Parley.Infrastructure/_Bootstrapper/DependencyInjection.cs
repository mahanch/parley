using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parley.Application._Shared.Behaviors;
using Parley.Application.Contracts.Interfaces.Caching;
using Parley.Application.Contracts.Interfaces.Infrastructure;
using Parley.Application.Contracts.Query.Conversation;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.MessageAgg;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Infrastructure._Shared.Services;
using Parley.Infrastructure.Persistence;
using Parley.Infrastructure.Persistence.QueryServices;
using Parley.Infrastructure.Persistence.Repositories;
using StackExchange.Redis;

namespace Parley.Infrastructure._Bootstrapper;

public static class DependencyInjection
{
      public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ParleyDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("postgres"),
                b => b.MigrationsAssembly(typeof(ParleyDbContext).Assembly.FullName)));

        // Redis
        var redisConfig = configuration.GetSection("Redis");
        var configOptions = new ConfigurationOptions
        {
            EndPoints = { $"{redisConfig["Host"]}:{redisConfig["Port"]}" },
            ConnectTimeout = int.Parse(redisConfig["ConnectionTimeout"]!),
            SyncTimeout = int.Parse(redisConfig["SyncTimeout"]!),
            AbortOnConnectFail = false  // فقط startup crash نکنه
        };
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configOptions));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();

        // Query Services
        services.AddScoped<IConversationQueryService, ConversationQueryService>();

        // Caching
        services.AddScoped<IRedisCache, RedisCache>();

        
        // Infrastructure Services
        services.AddSingleton<ISnowflakeIdGenerator, SnowflakeIdGenerator>();
        
        // MediatR Validation Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}