using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ExpressionCache.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionCacheTests.Extensions
{
    /// <summary>
    /// Summary description for StringExtensionsTest
    /// </summary>
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void ToHash_Test()
        {
            const string input = @"2 + 2";

            var hash = input.ToHash();

            Assert.IsNotNull(hash);
            Assert.IsTrue(hash.Length > 0);
            Assert.IsTrue(hash != input);
        }

        [TestMethod]
        public void Whitespace_Variations_Have_Same_Hash_Test()
        {
            const string input1 = "2 + 2";
            const string input2 = "2+2";
            const string input3 = "2                     +               2";
            const string input4 = "2\t+\t2";
            const string input5 = "2\r\n+\r\n2";

            var hashedInputs = new[] {input1.ToHash(), input2.ToHash(), input3.ToHash(), input4.ToHash(), input5.ToHash()};

            Assert.IsTrue(hashedInputs.All(x => x == input2.ToHash()));
        }

        [TestMethod]
        public void Hashing_Different_Values_Gives_Different_Hashes()
        {
            const string input1 = "1+1";
            const string input2 = "2+2";
            const string input3 = "1+2";

            Assert.IsFalse(
                new[] {input1.ToHash(), input2.ToHash(), input3.ToHash()}.All(x => x == input1));
        }
    }
}
