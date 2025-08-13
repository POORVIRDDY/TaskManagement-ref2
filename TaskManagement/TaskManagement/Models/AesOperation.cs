using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagement.Models
{
    public class AesOperation
    {
      
        private static readonly byte[] key = ConvertHexStringToByteArray("d1d7cdb88c8e6c6a7a5c2ab32b1a585cc1a234fa6e56f28c8a3e9ffbe6c2e582");

       
        private static readonly byte[] iv = new byte[] { 0x8d, 0xa7, 0xa4, 0xb6, 0xf2, 0x76, 0xf8, 0xf5, 0xd9, 0x12, 0xda, 0xc7, 0xb3, 0x52, 0xac, 0x8f };

       
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;  
                aesAlg.IV = iv;   

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());  
                }
            }
        }

       
        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;  
                aesAlg.IV = iv;  

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();  
                    }
                }
            }
        }

      
        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            int length = hexString.Length;
            byte[] byteArray = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return byteArray;
        }
    }
}