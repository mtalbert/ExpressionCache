using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionCache;
using Microsoft.Practices.Unity;
using Moq;

namespace ExpressionCacheTests
{
    internal static class UnityConfig
    {
        #region Unity Container

        private static readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            Expression<Func<bool>> alwaysTrue = () => true;
            Expression<Func<string, bool>> stringIsEmpty = s => s == string.Empty;

            var cachedItems = new List<IExpressionCacheItem>
            {
                new ExpressionCacheItem("2 + 2 = 4".CalculateMD5Hash(), alwaysTrue.Compile()),
                new ExpressionCacheItem("s = \"\"".CalculateMD5Hash(), stringIsEmpty.Compile())
            };

            container.RegisterType<IExpressionCache, ExpressionCache.ExpressionCache>(
                new PerResolveLifetimeManager(),
                new InjectionFactory(c =>
                {
                    var cache = new ExpressionCache.ExpressionCache();
                    cache.Add(cachedItems);

                    return cache;
                })
                );

            container.RegisterType<Mock<IExpressionCache>>(new PerResolveLifetimeManager(), new InjectionFactory(c =>
            {
                var expressionCache = new Mock<IExpressionCache>();

                var expressionCacheItems = cachedItems;

                expressionCache.Setup(x => x.List()).Returns(expressionCacheItems);

                expressionCache
                    .Setup(x => x.Get(It.IsAny<string>()))
                    .Returns<string>(
                        expression =>
                        {
                            return expressionCacheItems.Find(i => i.ExpressionHash.Equals(expression));
                        }
                    );

                expressionCache
                    .Setup(x => x.Add(It.IsAny<IExpressionCacheItem>()))
                    .Callback<IExpressionCacheItem>(cacheItem =>
                    {
                        expressionCacheItems.Add(cacheItem);
                    });

                expressionCache
                    .Setup(x => x.Add(It.IsAny<IEnumerable<IExpressionCacheItem>>()))
                    .Callback<IEnumerable<IExpressionCacheItem>>(cacheItems =>
                    {
                        foreach (var item in cacheItems)
                            expressionCache.Object.Add(item);
                    });

                expressionCache
                    .Setup(x => x.Remove(It.IsAny<string>()))
                    .Callback<string>(expression =>
                    {
                        expressionCacheItems.Remove(expressionCacheItems.Find(i => i.ExpressionHash == expression));
                    });

                expressionCache
                    .Setup(x => x.Remove(It.IsAny<IEnumerable<string>>()))
                    .Callback<IEnumerable<string>>(expressions =>
                    {
                        foreach (var expression in expressions)
                            expressionCache.Object.Remove(expression);
                    });

                expressionCache.Setup(x => x.Clear()).Callback(() => expressionCacheItems.Clear());

                return expressionCache;

            }));

        }
    }
}
