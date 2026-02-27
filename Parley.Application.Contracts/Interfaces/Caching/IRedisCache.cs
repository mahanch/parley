using StackExchange.Redis;

namespace Parley.Application.Contracts.Interfaces.Caching;

/// <summary>
/// Interface for Redis caching operations.
/// Supports SignalR Redis backplane for distributed caching.
/// </summary>
public interface IRedisCache
{
    /// <summary>
    /// Sets a value in Redis with optional expiration.
    /// </summary>
    Task SetAsync(string key, string value, TimeSpan? expiration = null);

    /// <summary>
    /// Gets a value from Redis.
    /// </summary>
    Task<string?> GetAsync(string key);

    /// <summary>
    /// Removes a value from Redis.
    /// </summary>
    Task DeleteAsync(string key);

    /// <summary>
    /// Checks if a key exists in Redis.
    /// </summary>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Gets the Redis connection for advanced operations.
    /// </summary>
    IConnectionMultiplexer GetConnection();
}

