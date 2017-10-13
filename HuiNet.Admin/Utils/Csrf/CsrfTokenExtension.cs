using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HuiNet.Admin.Utils.Csrf;

namespace System.Web.Mvc.Html
{
    public static class CsrfTokenExtension
    {
        public static MvcHtmlString CsrfToken(this HtmlHelper htmlHelper)
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentNullException("HttpContext");
            }

            HttpContext httpContext = HttpContext.Current;
            string oldToken = CsrfTokenWrapper.GetToken(httpContext);
            string newToken = oldToken;
            if (!CsrfTokenWrapper.IsTokenValid(oldToken))
            {
                newToken = CsrfTokenWrapper.GenerateToken(httpContext);
                // 如果生成了新token,保存token
                CsrfTokenWrapper.SaveToken(httpContext, newToken);
            }

            // <input type="hidden" id="__CsrfVerificationToken" name="__CsrfVerificationToken" value="..." />
            TagBuilder retVal = new TagBuilder("input");
            retVal.Attributes["type"] = "hidden";
            retVal.Attributes["id"] = CsrfConfig.GetTokenFieldName();
            retVal.Attributes["name"] = CsrfConfig.GetTokenFieldName();
            retVal.Attributes["value"] = newToken;

            string input = retVal.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(input);
        }

        public static MvcForm BeginTokenForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method, object htmlAttributes)
        {
            MvcForm form = htmlHelper.BeginForm(actionName, controllerName, method, htmlAttributes);

            MvcHtmlString csrf = htmlHelper.CsrfToken();
            htmlHelper.ViewContext.Writer.Write(csrf.ToHtmlString());

            return form;
        }
    }
}