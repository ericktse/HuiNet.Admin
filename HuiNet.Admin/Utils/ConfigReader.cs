using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Utils
{
    public class ConfigReader
    {
        /// <summary>
        /// API地址
        /// </summary>
        public static readonly string RouteGateWay = ConfigurationManager.AppSettings["RouteGateWay"].TrimEnd('/');

        /// <summary>
        /// 加密安全码
        /// </summary>
        public static readonly string RouteDesKey = ConfigurationManager.AppSettings["RouteDesKey"];
    }
}