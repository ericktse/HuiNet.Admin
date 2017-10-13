using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuiNet.Admin.Models.Home;
using HuiNet.Admin.Utils;

namespace HuiNet.Admin.Controllers
{
    [CheckLogin]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.LoginUser = LoginUser;
            return View();
        }

        public ActionResult Welcome()
        {
            ViewBag.LoginUser = LoginUser;
            return View();
        }

        public ActionResult Menu()
        {
            List<MenuModel> menus = new List<MenuModel>();


            MenuModel menu1 = new MenuModel();
            menu1.Title = "菜单1";

            MenuModel menu11 = new MenuModel();
            menu11.Title = "菜单11";
            menu11.Path = "";
            menu1.SubMenus.Add(menu11);

            MenuModel menu12 = new MenuModel();
            menu12.Title = "菜单12";
            menu12.Path = "";
            menu1.SubMenus.Add(menu12);
            menus.Add(menu1);


            MenuModel menu2 = new MenuModel();
            menu2.Title = "菜单2";

            MenuModel menu21 = new MenuModel();
            menu21.Title = "菜单21";
            menu21.Path = "";
            menu2.SubMenus.Add(menu21);

            MenuModel menu22 = new MenuModel();
            menu22.Title = "菜单22";
            menu22.Path = "";
            menu2.SubMenus.Add(menu22);
            menus.Add(menu2);

            return View("_Menu", menus);
        }
    }
}