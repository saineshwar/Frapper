using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Frapper.Repository.CacheService
{
    public class RedisCacheService : RedisCacheServiceBase, IRedisCacheService
    {
        public RedisCacheService(IDistributedCache distributedCache) : base(distributedCache)
        {

        }

        public void SetObject(string key, object objecttostore, DistributedCacheEntryOptions options)
        {
            if (key != string.Empty && objecttostore != null)
            {
                string serializeObject = JsonConvert.SerializeObject(objecttostore);
                byte[] data = Encoding.UTF8.GetBytes(serializeObject);
                DistributedCache.Set(key, data, options: options);
            }
        }

        public object GetObject<T>(string key)
        {
            if (key != string.Empty)
            {
                byte[] data = DistributedCache.Get(key);
                var bytesAsString = Encoding.UTF8.GetString(data);
                var deserializeObject = JsonConvert.DeserializeObject<T>(bytesAsString);
                return deserializeObject;
            }

            return null;
        }

        public void SetString(string key, string stringvalue, DistributedCacheEntryOptions options)
        {
            if (key != string.Empty && stringvalue != string.Empty && options != null)
            {
                DistributedCache.SetString(key, stringvalue, options);
            }
        }

        public string GetString(string key)
        {
            if (key != string.Empty)
            {
                var stringvalue = DistributedCache.GetString(key);
                return stringvalue;
            }
            return String.Empty;
        }

    }
}
