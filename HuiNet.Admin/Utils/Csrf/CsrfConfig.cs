using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace HuiNet.Admin.Utils.Csrf
{
    public class CsrfConfig
    {
        /// <summary>
        /// 是否启用Csrf检查,默认启动
        /// </summary>
        public static readonly bool IsEnable = string.Equals(ConfigurationManager.AppSettings["CsrfValidate"] ?? "true", "true", StringComparison.OrdinalIgnoreCase);

        private const string TokenFieldName = "__CsrfVerificationToken";

        public static string GetTokenFieldName()
        {
            var appPath = HttpRuntime.AppDomainAppVirtualPath;

            if (string.IsNullOrEmpty(appPath) || appPath == "/")
            {
                return TokenFieldName;
            }
            else
            {
                return TokenFieldName + "_" + HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(appPath));
            }
        }
    }
}