using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common;

using WebApiDemo2.Common;

namespace WebApiDemo2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cr = new ContentResult()
            {
                Content = "Api Version 0.1"
            };

            return cr;
        }
    }
}
