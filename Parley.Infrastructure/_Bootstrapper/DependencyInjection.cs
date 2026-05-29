using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Parley.Application._Shared.Behaviors;
using Parley.Application.Contracts.Interfaces.Caching;
using Parley.Application.Contracts.Interfaces.Infrastructure;
using Parley.Application.Contracts.Interfaces.Security;
using Parley.Application.Contracts.Query.Conversation;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.MessageAgg;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Infrastructure._Shared.Services;
using Parley.Infrastructure.Persistence;
using Parley.Infrastructure.Persistence.QueryServices;
using Parley.Infrastructure.Persistence.Repositories;
using Parley.Infrastructure.Security;
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
            AbortOnConnectFail = false
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

        // Storage Service (MinIO)
        var storageConfig = configuration.GetSection("Storage");
        services.AddSingleton<IMinioClient>(sp => 
        {
            return new MinioClient()
                .WithEndpoint(storageConfig["Endpoint"] ?? "localhost:9000")
                .WithCredentials(
                    storageConfig["AccessKey"] ?? "minioadmin",
                    storageConfig["SecretKey"] ?? "minioadmin")
                .WithSSL(Convert.ToBoolean(storageConfig["UseSSL"]))
                .Build();
        });
        services.AddScoped<IStorageService, Parley.Infrastructure.Services.Storage.MinioStorageService>();

        // Infrastructure Services
        services.AddSingleton<ISnowflakeIdGenerator, SnowflakeIdGenerator>();
        
        // MediatR Validation Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Auth Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthHelper, AuthHelper>();
        services.AddHttpContextAccessor();

        // JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secret = jwtSettings.GetValue<string>("Secret") ?? throw new Exception("JWT Secret not found");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
            };
        });

        services.AddAuthorization();

        return services;
    }
}
