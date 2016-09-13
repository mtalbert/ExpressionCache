using System.Collections.Generic;

namespace ExpressionCache
{
    public interface IExpressionCache
    {
        IEnumerable<IExpressionCacheItem> List();
        IExpressionCacheItem Get(string expressionHash);
        void Add(IExpressionCacheItem cacheItem);
        void Add(IEnumerable<IExpressionCacheItem> cacheItems);
        void Remove(string expressionHash);
        void Remove(IEnumerable<string> expressionHashes);
        void Clear();
    }
}
