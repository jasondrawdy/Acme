using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using AcmeCorpApi.Utils.Generators;

namespace AcmeCorpApi.Utils.Security
{
    public class Hashing
    {
        public enum HashType
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512,
        }

        private byte[] GetHash(string input, HashType hash)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            switch (hash)
            {
                case HashType.MD5:
                    return MD5.Create().ComputeHash(inputBytes);
                case HashType.SHA1:
                    return SHA1.Create().ComputeHash(inputBytes);
                case HashType.SHA256:
                    return SHA256.Create().ComputeHash(inputBytes);
                case HashType.SHA384:
                    return SHA384.Create().ComputeHash(inputBytes);
                case HashType.SHA512:
                    return SHA512.Create().ComputeHash(inputBytes);
                default:
                    return inputBytes;
            }
        }

        public string ComputeMessageHash(string input, HashType selection)
        {
            try
            {
                byte[] hash = GetHash(input, selection);
                StringBuilder result = new StringBuilder();

                for (int i = 0; i <= hash.Length - 1; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }
                return result.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public class Encryption
    {
        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }; // The salt bytes must be at least 8 bytes.

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes AES = Aes.Create())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    int iterations = 1000;
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
                    {
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        
                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        }

                        encryptedBytes = ms.ToArray();
                    }
                }
            }

            return encryptedBytes;
        }

        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }; // The salt bytes must be at least 8 bytes.

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes AES = Aes.Create())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    int iterations = 1000;
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
                    {
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }
            }

            return decryptedBytes;
        }

        public string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public string DecryptText(string data, string password)
        {
            try
            {
                byte[] bytesToBeDecrypted = Convert.FromBase64String(data);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                string result = Encoding.UTF8.GetString(bytesDecrypted);

                return result;
            }
            catch { return null; }
        }
        
        public string EncryptTextSalted(string text, string password, string salt = null)
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256.
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes.
            byte[] saltBytes;
            if (salt != null)
                saltBytes = Convert.FromBase64String(salt);
            else
                saltBytes = new SecureRandom().GetBytes(8);

            // Appending salt bytes to original bytes.
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
                bytesToBeEncrypted[i] = saltBytes[i];

            for (int i = 0; i < originalBytes.Length; i++)
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];

            byte[] encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string DecryptTextSalted(string data, string password)
        {
            try
            {
                byte[] bytesToBeDecrypted = Convert.FromBase64String(data);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256.
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                // Getting the size of salt.
                int _saltSize = 8;

                // Removing salt bytes, retrieving original bytes.
                byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
                for (int i = _saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - _saltSize] = decryptedBytes[i];
                }

                return Encoding.UTF8.GetString(originalBytes);
            }
            catch { return null; }
        }
    }
}