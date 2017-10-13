using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuiNet.Admin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult InternalServer(string message)
        {
            ViewBag.Message = message;
            return View("Error");
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        public ActionResult Forbidden()
        {
            return View("Forbidden");
        }
    }
}