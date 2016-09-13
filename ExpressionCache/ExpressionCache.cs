using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace ExpressionCache
{
    public class ExpressionCache : IExpressionCache, IDisposable
    {
        private readonly ObjectCache _cache;
        private readonly ExpressionCacheOptions _options;

        public ExpressionCache(ObjectCache cache = null, ExpressionCacheOptions options = null)
        {
            _options = options ?? new ExpressionCacheOptions();

            _cache = cache ?? new MemoryCache("ExpressionCache", new NameValueCollection
            {
                {"cacheMemoryLimitMegabytes", _options.MemoryLimitSize.ToString()},
                {"physicalMemoryLimitPercentage", _options.MemoryLimitPercent.ToString()},
                {"pollingInterval", _options.PollingInterval.ToString()}
            });
        }

        public ExpressionCache(IEnumerable<IExpressionCacheItem> initialItems, ObjectCache cache = null, ExpressionCacheOptions options = null) : this(cache, options)
        {
            Add(initialItems);
        }

        public IEnumerable<IExpressionCacheItem> List()
        {
            return _cache.Select(x => new ExpressionCacheItem(x.Key, x.Value as Delegate)).ToArray();
        }

        public IExpressionCacheItem Get(string expressionHash)
        {
            var cacheItem = _cache.Get(expressionHash) as Delegate;
            return cacheItem == null ? null : new ExpressionCacheItem(expressionHash, cacheItem);
        }

        public void Add(IExpressionCacheItem cacheItem)
        {
            _cache.Add(cacheItem.ExpressionHash, cacheItem.CompiledExpression, _options.CacheItemPolicy);
        }

        public void Add(IEnumerable<IExpressionCacheItem> cacheItems)
        {
            Parallel.ForEach(cacheItems, Add);
        }

        public void Remove(string expressionHash)
        {
            _cache.Remove(expressionHash);
        }

        public void Remove(IEnumerable<string> expressionHashes)
        {
            Parallel.ForEach(expressionHashes, h => _cache.Remove(h));
        }

        public void Clear()
        {
            var keys = _cache.Select(o => o.Key);
            Parallel.ForEach(keys, k => _cache.Remove(k));
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    var disposableCache = _cache as IDisposable;

                    disposableCache?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
