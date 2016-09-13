using System;
using System.Runtime.Caching;

namespace ExpressionCache
{
    /// <summary>
    /// Options for the Expression Cache.  These options will be used to configure the internal ObjectCache.
    /// </summary>
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
        /// <summary>
        /// Memory Limit in MegaBytes.  Defaults to 100MB
        /// </summary>
        public int MemoryLimitSize {get; set;}
        /// <summary>
        /// Memory Limit Percentage.  Defaults to 20%
        /// </summary>
        public int MemoryLimitPercent { get; set; }
        /// <summary>
        /// Maximum interval in which the cache updates its memory statistics.
        /// </summary>
        public TimeSpan PollingInterval { get; set; }
        /// <summary>
        /// Cache Item Policy used to expire expressions. Default is a 4 hour sliding expiration.
        /// </summary>
        public CacheItemPolicy CacheItemPolicy { get; set; }
    }
}
