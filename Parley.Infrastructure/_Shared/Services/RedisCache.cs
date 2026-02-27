using Parley.Application.Contracts.Interfaces.Caching;
using StackExchange.Redis;

namespace Parley.Infrastructure._Shared.Services;


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

