using System;

namespace ExpressionCache
{
    public class ExpressionCacheItem : IExpressionCacheItem
    {
        public ExpressionCacheItem(string expressionHash, Delegate compiledExpression)
        {
            if (string.IsNullOrEmpty(expressionHash))
                throw new ArgumentNullException(nameof(expressionHash));

            if (compiledExpression == null)
                throw new ArgumentNullException(nameof(compiledExpression));

            ExpressionHash = expressionHash;
            CompiledExpression = compiledExpression;
        }

        public string ExpressionHash { get; }
        public Delegate CompiledExpression { get; }
    }
}
