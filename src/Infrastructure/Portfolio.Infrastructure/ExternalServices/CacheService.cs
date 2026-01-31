using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Portfolio.Application.Abstraction.Infrastructure;

namespace Portfolio.Infrastructure.ExternalServices;
public class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getData, TimeSpan expiration)
    {
        var cachedData = await cache.GetStringAsync(key); 
        
        if(!string.IsNullOrWhiteSpace(cachedData))
            return JsonSerializer.Deserialize<T>(cachedData)!;

        var data = await getData();
        
        if(data is not null) 
            await  cache.SetStringAsync(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration 
                
            });
        return data;
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}