using Application.Interfaces.Cache;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;
    
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    
    public async Task<string?> GetAsync(string key)
    {
        var value = await _database.StringGetAsync(key);

        if (!value.HasValue)
        {
            return null;
        }
        
        return value.ToString();
    }
    
    public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        await _database.StringSetAsync(key, value, expiry);
    }
    
    public async Task DeleteAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
    
    public async Task IncrementAsync(string key)
    {
        await _database.StringIncrementAsync(key);
    }
}