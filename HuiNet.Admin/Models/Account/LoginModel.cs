using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuiNet.Admin.Models.Account
{
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string VidateCode { get; set; }
    }
}