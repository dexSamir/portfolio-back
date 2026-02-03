using Microsoft.Extensions.Caching.Memory;
using Portfolio.Application.Abstraction.Infrastructure;

namespace Portfolio.Infrastructure.ExternalServices;
public class CacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> getData,
        TimeSpan expiration)
    {
        if (cache.TryGetValue(key, out T? cachedValue))
            return cachedValue!;

        var data = await getData();

        if (data is not null)
        {
            cache.Set(
                key,
                data,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                });
        }

        return data;
    }
    public Task RemoveAsync(string key)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }
}