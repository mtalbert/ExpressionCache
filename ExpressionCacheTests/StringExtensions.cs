using System.Text;
using System.Security.Cryptography;

namespace ExpressionCacheTests
{
    public static class StringExtensions
    {
        public static string CalculateMD5Hash(this string input)
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
