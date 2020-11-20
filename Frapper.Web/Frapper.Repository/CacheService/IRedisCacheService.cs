using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace Frapper.Repository.CacheService
{
    public interface IRedisCacheService
    {
        void SetObject(string key, object objecttostore, DistributedCacheEntryOptions options);
        void SetString(string key, string stringvalue, DistributedCacheEntryOptions options);
        object GetObject<T>(string key);
        string GetString(string key);
    }
}
