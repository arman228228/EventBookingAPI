namespace Application.Interfaces.Cache;

public interface ICacheService
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value, TimeSpan? expiry = null);
    Task DeleteAsync(string key);
    Task IncrementAsync(string key);
}