/******************************
 * Copyright (C) 2012  Ryan Perneel

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License  
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ******************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PasswordSafe
{
    public class Crypto
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("12345678");

        public static string Encrypt(string plainText, string shared)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException();
            if (string.IsNullOrEmpty(shared))
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

        public static string Decrypt (string cipher, string shared)
        {
            if (string.IsNullOrEmpty(cipher))
                throw new ArgumentNullException("cipher");
            if (string.IsNullOrEmpty(shared))
                throw new ArgumentNullException("shared");

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
