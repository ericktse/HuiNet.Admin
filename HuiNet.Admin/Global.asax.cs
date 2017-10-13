using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HuiNet.Admin.Utils;

namespace HuiNet.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Logger.Error("请求异常", ex.Message, ex);

            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            {
                Response.Redirect("~/Error/NotFound", true);
            }
            else if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 403)
            {
                Response.Redirect("~/Error/Forbidden", true);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Response.StatusDescription = ex.Message;
                Response.Redirect(string.Format("~/Error/InternalServer?Message={0}", ex.Message), true);
            }
        }
    }
}
