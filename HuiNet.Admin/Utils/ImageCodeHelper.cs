using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Utils
{
    public class ImageCodeHelper
    {
        private static string SessionKey = "ValidateCode";

        public static byte[] GenerateImageCode()
        {
            ImageCodeGenerator generator = new ImageCodeGenerator();

            string code = generator.CreateVerifyCode().ToUpper(); //取随机码 

            HttpContext.Current.Session[SessionKey] = code;//取得验证码，以便后来验证 

            // 输出图片 
            return generator.GenerateValidateCode(code);
        }

        public static bool ValidateImageCode(string code)
        {
            var sessionCode = HttpContext.Current.Session[SessionKey];
            if (string.Equals(sessionCode.ToString(), code, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetImageCode()
        {
            var code = HttpContext.Current.Session[SessionKey];
            if (code == null)
                return null;
            return code.ToString();
        }

        public static void RemoveImageCode()
        {
            HttpContext.Current.Session[SessionKey] = null;
        }
    }
}