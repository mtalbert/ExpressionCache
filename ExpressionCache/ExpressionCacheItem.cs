using System;

namespace ExpressionCache
{
    /// <summary>
    /// Item representing a Compiled Expression
    /// </summary>
    public class ExpressionCacheItem : IExpressionCacheItem
    {
        /// <summary>
        /// Creates an ExpressionCacheItem that can be stored in the ExpressionCache
        /// </summary>
        /// <param name="expressionHash">Hash used as a key in the Expression Hash</param>
        /// <param name="compiledExpression">A Compiled Expression</param>
        public ExpressionCacheItem(string expressionHash, Delegate compiledExpression)
        {
            if (string.IsNullOrEmpty(expressionHash))
                throw new ArgumentNullException(nameof(expressionHash));

            if (compiledExpression == null)
                throw new ArgumentNullException(nameof(compiledExpression));

            ExpressionHash = expressionHash;
            CompiledExpression = compiledExpression;
        }
        /// <summary>
        /// Hash of the original text expression.
        /// </summary>
        public string ExpressionHash { get; }
        /// <summary>
        /// The Compiled Expression
        /// </summary>
        public Delegate CompiledExpression { get; }
    }
}
