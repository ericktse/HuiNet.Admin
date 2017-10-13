using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace HuiNet.Admin.Utils
{
    public class EncryptHelper
    {
        public static string CertDecrypt(string enData, string cerPath, string cerPwd)
        {
            try
            {
                byte[] rgb = Convert.FromBase64String(enData);
                X509Certificate2 certificate = new X509Certificate2(cerPath, cerPwd);
                byte[] bytes = (certificate.PrivateKey as RSACryptoServiceProvider).Decrypt(rgb, true);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string CertEncrypt(string srcData, string cerPath)
        {
            try
            {
                X509Certificate2 certificate = new X509Certificate2(cerPath);
                RSACryptoServiceProvider key = certificate.PublicKey.Key as RSACryptoServiceProvider;
                byte[] bytes = Encoding.UTF8.GetBytes(srcData);
                return Convert.ToBase64String(key.Encrypt(bytes, true));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string DecodeBase64(string codeType, string code)
        {
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                return Encoding.GetEncoding(codeType).GetString(bytes);
            }
            catch
            {
                return code;
            }
        }

        public static string DESDecrypt(string encryptData, string encryPwd)
        {
            if (string.IsNullOrEmpty(encryptData) || string.IsNullOrEmpty(encryPwd))
            {
                return string.Empty;
            }
            string s = encryPwd.Substring(0, 8);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] rgbIV = Encoding.UTF8.GetBytes(s);
            byte[] buffer = new byte[encryptData.Length / 2];
            for (int i = 0; i < (encryptData.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(encryptData.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                Encoding encoding = new UTF8Encoding();
                return encoding.GetString(stream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        public static string DESEncrypt(string srcData, string encryPwd)
        {
            if (string.IsNullOrEmpty(srcData) || string.IsNullOrEmpty(encryPwd))
            {
                return string.Empty;
            }
            string s = encryPwd.Substring(0, 8);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] rgbIV = Encoding.UTF8.GetBytes(s);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] buffer = Encoding.UTF8.GetBytes(srcData);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num2 in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num2);
            }
            return builder.ToString();
        }

        public static string EncodeBase64(string codeType, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "";
            }
            byte[] bytes = Encoding.GetEncoding(codeType).GetBytes(code);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return code;
            }
        }

        public static string GetMd5Hash(string input)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string SignDataForCert(string srcData, string xpath)
        {
            X509Certificate2 certificate = new X509Certificate2(xpath, "ucs2013");
            RSACryptoServiceProvider privateKey = certificate.PrivateKey as RSACryptoServiceProvider;
            byte[] bytes = Encoding.UTF8.GetBytes(srcData);
            return Convert.ToBase64String(privateKey.SignData(bytes, "SHA1"));
        }

        public static bool VerifyDataForCert(string srcData, string token, string xpath)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(srcData);
            X509Certificate2 certificate = new X509Certificate2(xpath);
            RSACryptoServiceProvider key = certificate.PublicKey.Key as RSACryptoServiceProvider;
            byte[] signature = Convert.FromBase64String(token);
            return key.VerifyData(bytes, new SHA1CryptoServiceProvider(), signature);
        }
    }
}