using System;

namespace ExpressionCache
{
    public interface IExpressionCacheItem
    {
        string ExpressionHash { get; }
        Delegate CompiledExpression { get; }
    }
}
