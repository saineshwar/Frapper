using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Frapper.Common
{
    public class AesAlgorithm
    {
        private static readonly RijndaelManaged Rijndael = new RijndaelManaged();
        private static readonly System.Text.UnicodeEncoding UnicodeEncoding = new UnicodeEncoding();

        private const int ChunkSize = 128;
        private const string Base64Key = "ZVRoV21acTR0N3cheiVDKg==";
        private const string Base64Iv = "RChHK0tiUGRTZ1ZrWXAzcw==";

        private void InitializeRijndael()
        {
            Rijndael.Mode = CipherMode.CBC;
            Rijndael.Padding = PaddingMode.PKCS7;
        }

        public AesAlgorithm()
        {
            InitializeRijndael();

            Rijndael.KeySize = ChunkSize;
            Rijndael.BlockSize = ChunkSize;

            Rijndael.Key = Convert.FromBase64String(Base64Key);
            Rijndael.IV = Convert.FromBase64String(Base64Iv);
        }

        public AesAlgorithm(byte[] key, byte[] iv)
        {
            InitializeRijndael();

            Rijndael.Key = key;
            Rijndael.IV = iv;
        }

        public string Decrypt(byte[] cipher)
        {
            ICryptoTransform transform = Rijndael.CreateDecryptor();
            byte[] decryptedValue = transform.TransformFinalBlock(cipher, 0, cipher.Length);
            return UnicodeEncoding.GetString(decryptedValue);
        }

        public string DecryptFromBase64String(string base64Cipher)
        {
            return Decrypt(Convert.FromBase64String(base64Cipher));
        }

        public byte[] EncryptToByte(string plain)
        {
            ICryptoTransform encryptor = Rijndael.CreateEncryptor();
            byte[] cipher = UnicodeEncoding.GetBytes(plain);
            byte[] encryptedValue = encryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            return encryptedValue;
        }

        public string EncryptToBase64String(string plain)
        {
            return Convert.ToBase64String(EncryptToByte(plain));
        }

        public string GetKey()
        {
            return Convert.ToBase64String(Rijndael.Key);
        }

        public string GetIV()
        {
            return Convert.ToBase64String(Rijndael.IV);
        }

        public override string ToString()
        {
            return "KEY:" + GetKey() + Environment.NewLine + "IV:" + GetIV();
        }
    }
}
