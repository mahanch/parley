using StackExchange.Redis;

namespace Parley.Infrastructure._Shared.Services;

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

/// <summary>
/// Default Redis caching implementation.
/// </summary>
public class RedisCache : IRedisCache
{
    private readonly IConnectionMultiplexer _connection;

    public RedisCache(IConnectionMultiplexer connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        var db = _connection.GetDatabase();
        await db.StringSetAsync(key, value, expiration);
    }

    public async Task<string?> GetAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        var db = _connection.GetDatabase();
        var value = await db.StringGetAsync(key);

        return value.HasValue ? value.ToString() : null;
    }

    public async Task DeleteAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        var db = _connection.GetDatabase();
        await db.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        var db = _connection.GetDatabase();
        return await db.KeyExistsAsync(key);
    }

    public IConnectionMultiplexer GetConnection()
    {
        return _connection;
    }
}

