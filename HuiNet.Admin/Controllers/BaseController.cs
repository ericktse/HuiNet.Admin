using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuiNet.Admin.Models.Account;
using HuiNet.Admin.Models.Common;
using HuiNet.Admin.Utils;

namespace HuiNet.Admin.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo LoginUser
        {
            get
            {
                return AuthorizeProvider.GetAuthorize(HttpContext);
            }
        }

        public BaseResponse TryCatch(Action action, BaseResponse response = null)
        {
            if (response == null)
            {
                response = new BaseResponse();
            }

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Logger.Error(Convert.ToString(action), Convert.ToString(ex));
                response.SetFail(ex.ToString());
            }

            response.SetSuccess();

            return response;
        }
    }
}