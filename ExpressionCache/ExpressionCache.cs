using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace ExpressionCache
{
    /// <summary>
    /// Cache for storing Compiled Expressions by a hash of the text representation.
    /// </summary>
    public class ExpressionCache : IExpressionCache
    {
        private readonly ObjectCache _cache;
        private readonly ExpressionCacheOptions _options;

        /// <summary>
        /// Creates an Expression Cache to cache compiled expressions by hash.
        /// </summary>
        /// <param name="cache">Optional ObjectCache to use as the internal cache.  Default uses a MemoryCache.</param>
        /// <param name="options">The ExpressionCacheOptions used to configure the internal ObjectCache</param>
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

        /// <summary>
        /// List the contents of the Expression Cache
        /// </summary>
        /// <returns>List of Expression Cache Items representing the current contents of the cache.</returns>
        public IEnumerable<IExpressionCacheItem> List()
        {
            return _cache.Select(x => new ExpressionCacheItem(x.Key, x.Value as Delegate)).ToArray();
        }

        /// <summary>
        /// Get a specific Compiled Expression by hash.
        /// </summary>
        /// <param name="expressionHash">The hashed value of the original text expression.</param>
        /// <returns>An Expression Cache Item found for the given hash.</returns>
        public IExpressionCacheItem Get(string expressionHash)
        {
            var compiledExpression = _cache.Get(expressionHash) as Delegate;
            return compiledExpression == null ? null : new ExpressionCacheItem(expressionHash, compiledExpression);
        }

        /// <summary>
        /// Adds an Expression Cache Item to the cache.
        /// </summary>
        /// <param name="cacheItem">The Expression Cache Item to add to the cache.</param>
        public bool Add(IExpressionCacheItem cacheItem)
        {
            return _cache.Add(cacheItem.ExpressionHash, cacheItem.CompiledExpression, _options.CacheItemPolicy);
        }

        /// <summary>
        /// Adds multiple Expression Cache Items to the cache.
        /// </summary>
        /// <param name="cacheItems">The Expression Cache Items to add to the cache.</param>
        public void Add(IEnumerable<IExpressionCacheItem> cacheItems)
        {
            Parallel.ForEach(cacheItems, c => Add(c));
        }

        /// <summary>
        /// Remove an Expression Cached Item from the cache by hash
        /// </summary>
        /// <param name="expressionHash">The hashed value of the original text expression.</param>
        /// <returns>The Expression Cache Item just removed from the collection.</returns>
        public IExpressionCacheItem Remove(string expressionHash)
        {
            var compiledExpression = _cache.Remove(expressionHash) as Delegate;
            return compiledExpression == null ? null : new ExpressionCacheItem(expressionHash, compiledExpression);
        }

        /// <summary>
        /// Removes multiple Expression Cache Items from the cache by their hash.
        /// </summary>
        /// <param name="expressionHashes">The hashes of the items to be removed.</param>
        public void Remove(IEnumerable<string> expressionHashes)
        {
            Parallel.ForEach(expressionHashes, h => Remove(h));
        }

        /// <summary>
        /// Empty the Expression Cache of all items.
        /// </summary>
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

        /// <summary>
        /// Dispose of all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
