using System.Security.Cryptography;
using System.Text;

namespace program.Helpers
{
    public static class SecurityHelper
    {
        public static byte[] HashSHA1(string plainText)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        }
        public static (byte[] encryptedData, string publicKey, string privateKey) EncryptRSA(string rawData)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                string publicKey = rsa.ToXmlString(false);
                string privateKey = rsa.ToXmlString(true);

                byte[] dataBytes = Encoding.UTF8.GetBytes(rawData);
                byte[] encryptedData = rsa.Encrypt(dataBytes, false);

                return (encryptedData, publicKey, privateKey);
            }

        }
        // Encrypt rawData using a provided RSA key (public or private XML).
        public static byte[] EncryptRSAWithKey(string rawData, string keyXml)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(keyXml);
                byte[] dataBytes = Encoding.UTF8.GetBytes(rawData);
                return rsa.Encrypt(dataBytes, false);
            }
        }
        public static string DecryptRSA(byte[] encryptedData, string privateKeyXml)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKeyXml);
                byte[] decryptedBytes = rsa.Decrypt(encryptedData, false);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }

}