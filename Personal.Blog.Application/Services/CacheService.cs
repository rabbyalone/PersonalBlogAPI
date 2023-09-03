using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace Personal.Blog.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan expirationTime)
        {
            if (_memoryCache.TryGetValue(key, out T cachedItem))
            {
                return cachedItem;
            }

            T item = await getItemCallback();

            if (item != null)
            {
                _memoryCache.Set(key, item, expirationTime);
            }

            return item;
        }

        public async Task CreateOrUpdateAsync<T>(string key, T newItem, TimeSpan expirationTime)
        {
            _memoryCache.Set(key, newItem, expirationTime);
            await Task.Yield(); // Async operation to match the method signature.
        }

        public async Task DeleteAsync(string key)
        {
            _memoryCache.Remove(key);
            await Task.Yield(); // Async operation to match the method signature.
        }

        //public async Task<IEnumerable<string>> GetAllKeysAsync()
        //{
        //    var cacheEntriesCollection = _memoryCache.GetKeys();
        //    return await Task.FromResult(cacheEntriesCollection);
        //}


    }
}
