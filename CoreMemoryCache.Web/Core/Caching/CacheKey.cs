using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CoreMemoryCache.Web.Core.Caching
{
    public class CacheKey
    {
        #region Ctor
        public CacheKey(string key, int cacheTime)
        {
            Key = key;
            CacheTime = cacheTime;
        }
        public CacheKey(string key, int cacheTime, int cacheSlidingTime)
        {
            Key = key;
            CacheTime = cacheTime;
            CacheSlidingTime = cacheSlidingTime;
        }
        public CacheKey(string key, int cacheTime, int cacheSlidingTime, CacheItemPriority cacheItemPriority)
        {
            Key = key;
            CacheTime = cacheTime;
            CacheSlidingTime = cacheSlidingTime;
            CacheItemPriority = cacheItemPriority;
        }
        #endregion

        public string Key { get; protected set; }
        public int CacheTime { get; set; }
        public int? CacheSlidingTime { get; set; }
        public CacheItemPriority? CacheItemPriority { get; set; }
    }
}
