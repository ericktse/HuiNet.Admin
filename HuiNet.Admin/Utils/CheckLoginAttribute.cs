using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuiNet.Admin.Models.Account;

namespace HuiNet.Admin.Utils
{
    public class CheckLoginAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null || httpContext.Session == null)
            {
                return false;
            }
            var sessionUser = AuthorizeProvider.GetAuthorize(httpContext);
            if (sessionUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult()
                {
                    Data = new { logoff = true, url = "/Account/Login", message = "请求超时，请重新登录", code = 401 },
                    ContentType = null,
                    ContentEncoding = null,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}