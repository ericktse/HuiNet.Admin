using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace HuiNet.Admin.Utils.Csrf
{
    public class CsrfTokenWrapper
    {
        public static string GetToken(HttpContext httpContext)
        {
            try
            {
                object session = httpContext.Session[CsrfConfig.GetTokenFieldName()];
                if (session == null)
                {
                    // did not exist
                    return null;
                }

                return session.ToString();
            }
            catch
            {
                // ignore failures since we'll just generate a new token
                return null;
            }
        }

        public static string GetFormToken(HttpContext httpContext)
        {
            string name = CsrfConfig.GetTokenFieldName();
            string value = httpContext.Request.Headers[name] ?? httpContext.Request.Form[name] ?? httpContext.Request.QueryString[name];
            if (String.IsNullOrEmpty(value))
            {
                // did not exist
                return null;
            }

            return value;
        }

        public static bool IsTokenValid(string token)
        {
            return !string.IsNullOrEmpty(token);
        }

        public static string GenerateToken(HttpContext httpContext)
        {
            string tokenKey = httpContext.Session.SessionID + DateTime.Now.Ticks;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(tokenKey));
            string token = BitConverter.ToString(b).Replace("-", string.Empty);

            return token;
        }

        public static void SaveToken(HttpContext httpContext, string token)
        {
            string name = CsrfConfig.GetTokenFieldName();
            httpContext.Session[name] = token;
        }

        public static bool ValidateToken(string sessionToken, string fieldToken)
        {
            if (string.IsNullOrEmpty(sessionToken))
            {
                return false;
            }
            if (string.IsNullOrEmpty(fieldToken))
            {
                return false;
            }

            if (sessionToken == fieldToken)
            {
                return true;
            }

            return false;
        }
    }
}