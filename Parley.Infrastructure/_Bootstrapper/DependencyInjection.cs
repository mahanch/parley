using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parley.Application._Shared.Behaviors;
using Parley.Application.Contracts.Interfaces.Caching;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.UserAgg;
using Parley.Infrastructure._Shared.Services;
using Parley.Infrastructure.Persistence;
using Parley.Infrastructure.Persistence.Repositories;

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
                b => b.MigrationsAssembly(typeof(AppContext).Assembly.FullName)));
        
        
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Users
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRedisCache, RedisCache>();
        

        


        //  MediatR Validation Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        return services;
    }
}