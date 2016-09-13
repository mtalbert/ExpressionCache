using System;

namespace ExpressionCache
{
    /// <summary>
    /// Expression Cache Item Interface
    /// </summary>
    public interface IExpressionCacheItem
    {
        /// <summary>
        /// Hash of the original text expression.
        /// </summary>
        string ExpressionHash { get; }
        /// <summary>
        /// The Compiled Expression
        /// </summary>
        Delegate CompiledExpression { get; }
    }
}
