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
    public class AccountController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 登录（界面）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            Session["ValidateCode"] = null;

            return View("Login");
        }

        /// <summary>
        /// 登录（请求）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(LoginModel model)
        {
            var result = new BaseResponse();

            if (string.IsNullOrEmpty(model.UserName))
            {
                result.SetFail("请输入用户名");
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                result.SetFail("请输入密码");
                return Json(result);
            }
            if (string.IsNullOrEmpty(model.VidateCode))
            {
                result.SetFail("请输入验证码");
                return Json(result);
            }

            if (ImageCodeHelper.GetImageCode() == null)
            {
                result.SetFail("验证码已过期,请刷新验证码");
                return Json(result);
            }
            else
            {
                if (!ImageCodeHelper.ValidateImageCode(model.VidateCode.ToUpper()))
                {
                    result.SetFail("验证码错误");
                    return Json(result);
                }
            }

            // API登录逻辑

            // 登录成功
            UserInfo userInfo = new UserInfo();
            userInfo.UserId = Guid.NewGuid().ToString();
            userInfo.UserName = "Test_Admin";
            AuthorizeProvider.StoreAuthorize(userInfo, HttpContext);
            result.SetSuccess("登录成功");

            return Json(result);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            AuthorizeProvider.RemoveAuthorize(HttpContext);

            return RedirectToAction("Index", "Account");
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCheckCode()
        {
            var image = ImageCodeHelper.GenerateImageCode();

            return File(image, "image/jpeg");
        }
    }
}