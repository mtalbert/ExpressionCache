using System;
using System.Text;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionCache;
using ExpressionCache.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionCacheTests
{
    /// <summary>
    /// Summary description for ExpressionCacheItemTests
    /// </summary>
    [TestClass]
    public class ExpressionCacheItemTests
    {
        private readonly Expression<Func<bool>> _alwaysTrue = () => true;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_Create_ExpressionCacheItem_With_Null_Expression()
        {
            var expressionCacheItem = new ExpressionCacheItem(null, _alwaysTrue.Compile());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_Create_ExpressionCacheItem_With_Null_CompiledExpression()
        {
            var expressionCacheItem = new ExpressionCacheItem("CacheItemExpression".ToHash(), null);
        }
    }
}
