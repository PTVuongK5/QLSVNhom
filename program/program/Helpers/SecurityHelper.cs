using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.ApplicationServices;

namespace program.Helpers
{
    public static class SecurityHelper
    {
        public static byte[] HashSHA1(string plainText)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        }

        public static void CreateAsymmetricKey(string password)
        {
            using var rsa = RSA.Create(2048);
            var publicKey = rsa.ExportRSAPublicKey();
            var privateKey = rsa.ExportRSAPrivateKey();
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt, 
                100000, 
                HashAlgorithmName.SHA256
            );
            // Lấy 32 bytes (256 bits) làm khóa
            byte[] aesKey = pbkdf2.GetBytes(32); 
            using var aes = Aes.Create();
            aes.Key = aesKey;
            aes.GenerateIV();
            byte[] iv = aes.IV;
            // Mã hóa privateKey bằng AES
            using var encryptor = aes.CreateEncryptor();
            byte[] encryptedPrivateKey =
            encryptor.TransformFinalBlock(
                privateKey,
                0,
                privateKey.Length
            );

            //lưu publicKey, encryptedPrivateKey, salt, iv vào file
            Directory.CreateDirectory("user");
            File.WriteAllBytes(
            $"user/public.key",
            publicKey
            );

            File.WriteAllBytes(
                $"user/private.enc",
                encryptedPrivateKey
            );

            File.WriteAllBytes(
                $"user/salt.bin",
                salt
            );

            File.WriteAllBytes(
                $"user/iv.bin",
                iv
            );
        }

        public static byte[] encryptDataWithPublicKey(string data)
        {
            byte[] publicKey = File.ReadAllBytes("user/public.key");
            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(publicKey, out _);
            byte[] encryptedData = rsa.Encrypt(
                Encoding.UTF8.GetBytes(data),
                RSAEncryptionPadding.OaepSHA256
            );
            return encryptedData;
        }
        
        public static string decryptDataWithPrivateKey(byte[] encryptedData, string password)
        {
            byte[] encryptedPrivateKey = File.ReadAllBytes("user/private.enc");
            byte[] salt = File.ReadAllBytes("user/salt.bin");
            byte[] iv = File.ReadAllBytes("user/iv.bin");
            var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256
            );
            byte[] aesKey = pbkdf2.GetBytes(32);
            using var aes = Aes.Create();
            aes.Key = aesKey;
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor();
            byte[] privateKey = decryptor.TransformFinalBlock(
                encryptedPrivateKey,
                0,
                encryptedPrivateKey.Length
            );
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKey, out _);
            byte[] decryptedData = rsa.Decrypt(
                encryptedData,
                RSAEncryptionPadding.OaepSHA256
            );
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}