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
        public static byte[] EncryptAES(string plainText, string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(32);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            byte[] iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            byte[] result = new byte[salt.Length + iv.Length + cipherBytes.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(iv, 0, result, salt.Length, iv.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, salt.Length + iv.Length, cipherBytes.Length);

            return result;
        }
        public static string DecryptAES(byte[] encryptedPayload, string password)
        {
            // Cục payload tối thiểu phải có 16 byte Salt + 16 byte IV = 32 byte
            if (encryptedPayload == null || encryptedPayload.Length <= 32)
                throw new ArgumentException("Dữ liệu mã hóa không hợp lệ hoặc bị hỏng.");

            byte[] salt = new byte[16];
            byte[] iv = new byte[16];
            byte[] cipherBytes = new byte[encryptedPayload.Length - 32];

            Buffer.BlockCopy(encryptedPayload, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(encryptedPayload, salt.Length, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedPayload, salt.Length + iv.Length, cipherBytes, 0, cipherBytes.Length);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(32);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }

}