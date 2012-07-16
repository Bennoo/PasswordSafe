using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ConsoleApplication1
{
    public class Crypto_New
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("12345678");
        private static string shared = "SharedKey";

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException();
            
            string outStr = null;

            RijndaelManaged alg = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(shared, _salt);
                alg = new RijndaelManaged();
                alg.Key = key.GetBytes(alg.KeySize / 8);
                ICryptoTransform encryptor = alg.CreateEncryptor(alg.Key, alg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(alg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(alg.IV, 0, alg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (alg != null)
                    alg.Clear();
            }

            return outStr;
        }

        public static string Decrypt(string cipher)
        {
            if (string.IsNullOrEmpty(cipher))
                throw new ArgumentNullException("cipher");
            
            RijndaelManaged alg = null;
            string plaintext = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(shared, _salt);
                byte[] bytes = Convert.FromBase64String(cipher);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    alg = new RijndaelManaged();
                    alg.Key = key.GetBytes(alg.KeySize / 8);
                    alg.IV = ReadByteArray(msDecrypt);
                    ICryptoTransform decryptor = alg.CreateDecryptor(alg.Key, alg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            finally
            {
                if (alg != null)
                    alg.Clear();
            }
            return plaintext;
        }

        private static byte[] ReadByteArray(MemoryStream msDecrypt)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (msDecrypt.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }
            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (msDecrypt.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }
            return buffer;
        }
    }
}
