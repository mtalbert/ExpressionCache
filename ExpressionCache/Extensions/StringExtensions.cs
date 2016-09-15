using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionCache.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex _regex = new Regex(@"\s+");

        /// <summary>
        /// Creates a consistent hash that ignores whitespace formatting of the expression.
        /// </summary>
        /// <param name="input">The textual representation of the expression to hash.</param>
        /// <returns></returns>
        public static string ToHash(this string input)
        {
            return _regex.Replace(input, string.Empty).CalculateMD5Hash();
        }

        /// <summary>
        /// Calculates and MD5 Hash of the input
        /// </summary>
        /// <param name="input">The input to generate the MD5 Hash of.</param>
        /// <returns></returns>
        internal static string CalculateMD5Hash(this string input)
        {
            var bytes = Encoding.Unicode.GetBytes(input);

            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(bytes);

                var result = new StringBuilder();

                foreach (var hashByte in hashBytes)
                {
                    result.Append(hashByte.ToString("X2"));
                }

                return result.ToString();
            }
        }
    }
}
