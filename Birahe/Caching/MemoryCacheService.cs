namespace Birahe.EndPoint.Caching;

using Microsoft.Extensions.Caching.Memory;

public class MemoryCacheService {
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache) {
        _cache = cache;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? slidingExpiration = null) {
        if (_cache.TryGetValue(key, out T cachedValue)) {
            return cachedValue;
        }

        var value = await factory();

        var options = new MemoryCacheEntryOptions {
            SlidingExpiration = slidingExpiration ?? TimeSpan.FromSeconds(30)
        };

        _cache.Set(key, value, options);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null) {
        var options = new MemoryCacheEntryOptions {
            SlidingExpiration = slidingExpiration ?? TimeSpan.FromSeconds(30)
        };

        _cache.Set(key, value, options);
    }

    // Optional: remove cache
    public void Remove(string key) {
        _cache.Remove(key);
    }
}