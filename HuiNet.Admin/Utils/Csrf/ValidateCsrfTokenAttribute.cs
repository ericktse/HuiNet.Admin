using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HuiNet.Admin.Models.Common;
using HuiNet.Admin.Utils.Csrf;

namespace System.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateCsrfTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!CsrfWrapper.Validate())
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    BaseResponse data = new BaseResponse()
                    {
                        IsSuccess = false,
                        Message = "您此次操作被禁止,请稍后再试或联系管理员",
                        Code = (int)HttpStatusCode.Forbidden
                    };

                    filterContext.Result = new JsonResult()
                    {
                        Data = data,
                        ContentType = null,
                        ContentEncoding = null,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                    filterContext.HttpContext.Response.Clear();
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new RedirectResult("~/Error/Forbidden");
                }
            }
        }
    }
}