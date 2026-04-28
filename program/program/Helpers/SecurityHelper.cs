using System.Security.Cryptography;
using System.Text;

namespace QLSVNhom.Helpers
{
    public static class SecurityHelper
    {
        public static byte[] HashSHA1(string plainText)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        }
    }
}