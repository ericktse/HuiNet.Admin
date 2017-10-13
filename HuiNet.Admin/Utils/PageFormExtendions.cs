using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class PageFormExtendions
    {
        public static MvcForm BeginPageForm(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            return htmlHelper.BeginForm(actionName, controllerName, new RouteValueDictionary { { "PageIndex", "" } },
                FormMethod.Get);
        }

        public static MvcForm BeginPageForm(this HtmlHelper htmlHelper, string actionName, string controllerName, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.BeginForm(actionName, controllerName, new RouteValueDictionary { { "PageIndex", "" } },
                FormMethod.Get, htmlAttributes);
        }
    }
}