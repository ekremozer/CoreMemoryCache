using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMemoryCache.Web.Core.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Set(CacheKey cacheKey, object model);
        Task SetAsync(CacheKey cacheKey, object model);
        T GetOrCreate<T>(CacheKey cacheKey, Func<T> acquire);
        Task<T> GetOrCreateAsync<T>(CacheKey cacheKey, Func<T> acquire);
        void Remove(string key);
        Task RemoveAsync(string key);
    }
}
