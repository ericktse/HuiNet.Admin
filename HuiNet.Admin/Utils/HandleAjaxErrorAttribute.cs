using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HuiNet.Admin.Models.Common;

namespace HuiNet.Admin.Utils
{
    public class HandleAjaxErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                Logger.Error(string.Format("{0}请求异常", filterContext.RequestContext.HttpContext.Request.Url), filterContext.Exception.Message, filterContext.Exception);

                BaseResponse data = new BaseResponse()
                {
                    IsSuccess = false,
                    Message = string.Format("请求异常，{0}", filterContext.Exception.Message),
                    Code = (int)HttpStatusCode.InternalServerError
                };

                filterContext.Result = new JsonResult()
                {
                    Data = data,
                    ContentType = null,
                    ContentEncoding = null,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}