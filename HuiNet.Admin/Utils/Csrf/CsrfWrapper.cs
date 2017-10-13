using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Utils.Csrf
{
    public class CsrfWrapper
    {
        public static bool Validate()
        {
            if (CsrfConfig.IsEnable)
            {
                if (HttpContext.Current == null)
                {
                    throw new ArgumentNullException("HttpContext");
                }

                HttpContext httpContext = HttpContext.Current;

                // 提取Session Token 和表单Token
                string sessionToken = CsrfTokenWrapper.GetToken(httpContext);
                string formToken = CsrfTokenWrapper.GetFormToken(httpContext);

                // Validate
                bool isEqual = CsrfTokenWrapper.ValidateToken(sessionToken, formToken);

                return isEqual;
            }

            return true;
        }
    }
}