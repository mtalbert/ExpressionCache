using System.Collections.Generic;

namespace ExpressionCache
{
    public interface IExpressionCache
    {
        IEnumerable<IExpressionCacheItem> List();
        IExpressionCacheItem Get(string expressionHash);
        bool Add(IExpressionCacheItem cacheItem);
        void Add(IEnumerable<IExpressionCacheItem> cacheItems);
        IExpressionCacheItem Remove(string expressionHash);
        void Remove(IEnumerable<string> expressionHashes);
        void Clear();
    }
}
