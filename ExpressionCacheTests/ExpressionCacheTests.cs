using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionCache;
using ExpressionCache.Extensions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionCacheTests
{
    [TestClass]
    public class ExpressionCacheTests
    {
        private static IUnityContainer _unityContainer;
        private IExpressionCache _expressionCache;
        private readonly Expression<Func<bool>> _alwaysTrue = () => true;

        #region Init and Cleanup

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _unityContainer = UnityConfig.GetConfiguredContainer();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _unityContainer.Dispose();
        }

        [TestInitialize]
        public void Initialize()
        {
            _expressionCache = _unityContainer.Resolve<IExpressionCache>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _expressionCache.Dispose();
            _expressionCache = null;
        }
        
        #endregion

        [TestMethod]
        public void Can_List_Entries_In_Cache()
        {
            var cacheItems = _expressionCache.List();

            Assert.IsNotNull(cacheItems);
            Assert.IsTrue(cacheItems.Any());
        }

        [TestMethod]
        public void Can_Add_Entry_To_Cache()
        {
            const string expression = "1 + 1 = 2";

            var cacheItem = new ExpressionCacheItem(expression.ToHash(), _alwaysTrue.Compile());

            Assert.IsTrue(_expressionCache.Add(cacheItem));

            var cacheEntry = _expressionCache.Get(expression.ToHash());

            Assert.IsNotNull(cacheEntry);
        }

        [TestMethod]
        public void Can_Add_Multiple_Entries_To_Cache()
        {
            var itemsToBeCached = new List<IExpressionCacheItem>
            {
                new ExpressionCacheItem("Expression1".ToHash(), _alwaysTrue.Compile()),
                new ExpressionCacheItem("Expression2".ToHash(), _alwaysTrue.Compile()),
                new ExpressionCacheItem("Expression3".ToHash(), _alwaysTrue.Compile())
            };

            _expressionCache.Add(itemsToBeCached.ToArray());

            var cacheEntry1 = _expressionCache.Get(itemsToBeCached[0].ExpressionHash);
            var cacheEntry2 = _expressionCache.Get(itemsToBeCached[1].ExpressionHash);
            var cacheEntry3 = _expressionCache.Get(itemsToBeCached[2].ExpressionHash);

            Assert.IsNotNull(cacheEntry1);
            Assert.IsNotNull(cacheEntry2);
            Assert.IsNotNull(cacheEntry3);
        }

        [TestMethod]
        public void Can_Get_Single_Entry_From_Cache()
        {
            const string expression = "2 + 2 = 4";
            var cacheItem = _expressionCache.Get(expression.ToHash());
            Assert.IsNotNull(cacheItem);
        }

        [TestMethod]
        public void Can_Remove_Entry_From_Cache()
        {
            const string expression = "2 + 2 = 4";

            var cacheItem = _expressionCache.Remove(expression.ToHash());

            Assert.IsNotNull(cacheItem);
            Assert.IsNull(_expressionCache.Get(expression.ToHash()));
        }

        [TestMethod]
        public void Can_Remove_Multiple_Entries_From_Cache()
        {
            var entriesToRemove = _expressionCache.List().Take(2).Select(x => x.ExpressionHash).ToArray();

            _expressionCache.Remove(entriesToRemove);

            Assert.IsNull(_expressionCache.Get(entriesToRemove.First()));
            Assert.IsNull(_expressionCache.Get(entriesToRemove.Last()));
        }

        [TestMethod]
        public void Can_Clear_Cache()
        {
            _expressionCache.Clear();

            Assert.AreEqual(0, _expressionCache.List().Count());
        }
    }
}
