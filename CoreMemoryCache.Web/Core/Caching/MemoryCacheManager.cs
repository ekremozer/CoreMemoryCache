using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace CoreMemoryCache.Web.Core.Caching
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        { 
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T model);
            return model;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.Run(() =>
            {
                _memoryCache.TryGetValue(key, out T model);
                return model;
            });
        }
        public void Set(CacheKey cacheKey, object model)
        {
            var memoryCacheEntryOptions = PrepareMemoryCacheEntryOptions(cacheKey);
            _memoryCache.Set(cacheKey.Key, model, memoryCacheEntryOptions);
        }

        public Task SetAsync(CacheKey cacheKey, object model)
        {
            var memoryCacheEntryOptions = PrepareMemoryCacheEntryOptions(cacheKey);
            _memoryCache.Set(cacheKey.Key, model, memoryCacheEntryOptions);
            return Task.CompletedTask;
        }

        public T GetOrCreate<T>(CacheKey cacheKey, Func<T> acquire)
        {
            if (cacheKey.CacheTime <= 0 && (cacheKey.CacheSlidingTime == null || cacheKey.CacheSlidingTime <= 0))
            {
                return acquire();
            }

            var result = _memoryCache.GetOrCreate(cacheKey.Key, entry =>
            {
                entry.SetOptions(PrepareMemoryCacheEntryOptions(cacheKey));

                return acquire();
            });

            if (result == null)
            {
                Remove(cacheKey.Key);
            }

            return result;
        }

        public async Task<T> GetOrCreateAsync<T>(CacheKey cacheKey, Func<T> acquire)
        {
            if (cacheKey.CacheTime <= 0 && (cacheKey.CacheSlidingTime == null || cacheKey.CacheSlidingTime <= 0))
            {
                return acquire();
            }

            var result = _memoryCache.GetOrCreate(cacheKey.Key, entry =>
            {
                entry.SetOptions(PrepareMemoryCacheEntryOptions(cacheKey));

                return acquire();
            });

            if (result == null)
            {
                await RemoveAsync(cacheKey.Key);
            }

            return result;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        private static MemoryCacheEntryOptions PrepareMemoryCacheEntryOptions(CacheKey cacheKey)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheKey.CacheTime)
            };

            if (cacheKey.CacheSlidingTime > 0)
            {
                memoryCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes((int)cacheKey.CacheSlidingTime);
            }
            if (cacheKey.CacheItemPriority != null)
            {
                memoryCacheEntryOptions.Priority = (CacheItemPriority)cacheKey.CacheItemPriority;
            }

            return memoryCacheEntryOptions;
        }
    }
}
