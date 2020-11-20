using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace Frapper.Repository.CacheService
{
    public abstract class RedisCacheServiceBase
    {
        protected IDistributedCache DistributedCache { get; set; }

        protected RedisCacheServiceBase(IDistributedCache distributedCache)
        {
            DistributedCache = distributedCache;
        }
    }
}
