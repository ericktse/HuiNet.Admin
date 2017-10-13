using System.Web;
using System.Web.Mvc;
using HuiNet.Admin.Utils;

namespace HuiNet.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleAjaxErrorAttribute());
        }
    }
}
