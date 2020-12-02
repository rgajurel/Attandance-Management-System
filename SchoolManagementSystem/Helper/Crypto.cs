using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TechtonneMS
{
    public static class Crypto
    {

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "Xl5tY_vh7";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "Xl5tY_vh7";
            cipherText = cipherText.Replace(" ", "+");
            int mod4 = cipherText.Length % 4;
            if (mod4 > 0)
            {
                cipherText += new string('=', 4 - mod4);
            }
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string OneWayEncryter(string textToBeEncrypted)
        {
            if (!String.IsNullOrEmpty(textToBeEncrypted))
            {
                MD5 md5 = MD5.Create();
                byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(textToBeEncrypted);
                byte[] hash = md5.ComputeHash(bytes);

                StringBuilder sbPassword = new StringBuilder();
                for (int b = 0; b < hash.Length; b++)
                {
                    sbPassword.Append(hash[b].ToString("X2"));
                }

                return sbPassword.ToString();
            }
            else
            {
                return string.Empty;

            }
        }
    }
}