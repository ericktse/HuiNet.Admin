using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Models.Home
{
    public class MenuModel
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Path { get; set; }

        public List<MenuModel> SubMenus { get; set; }

        public MenuModel()
        {
            SubMenus = new List<MenuModel>();
        }
    }
}