using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HuiNet.Admin.Models.Account;

namespace HuiNet.Admin.Utils
{
    /// <summary>
    /// 用户凭证管理器
    /// </summary>
    public class AuthorizeProvider
    {
        public static void StoreAuthorize(UserInfo userInfo, HttpContextBase httpContext)
        {
            if (httpContext != null && httpContext.Session != null)
                httpContext.Session["Authorize"] = userInfo;
        }

        public static UserInfo GetAuthorize(HttpContextBase httpContext)
        {
            if (httpContext != null && httpContext.Session != null)
                return httpContext.Session["Authorize"] as UserInfo;
            return null;
        }

        public static void RemoveAuthorize(HttpContextBase httpContext)
        {
            if (httpContext != null && httpContext.Session != null)
            {
                //httpContext.Session.Remove("Authorize");
                httpContext.Session.Clear();
            }
        }
    }
}