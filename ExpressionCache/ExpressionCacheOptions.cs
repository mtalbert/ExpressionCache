using System;
using System.Runtime.Caching;

namespace ExpressionCache
{
    public class ExpressionCacheOptions
    {
        public ExpressionCacheOptions()
        {
            MemoryLimitSize = 100;
            MemoryLimitPercent = 20;
            PollingInterval = new TimeSpan(0,0,30);

            CacheItemPolicy = new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                SlidingExpiration = new TimeSpan(4, 0, 0)
            };
        }

        public int MemoryLimitSize {get; set;}
        public int MemoryLimitPercent { get; set; }
        public TimeSpan PollingInterval { get; set; }
        public CacheItemPolicy CacheItemPolicy { get; set; }
    }
}
