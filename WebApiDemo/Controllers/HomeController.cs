using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Text;

using Common;

using WebApiDemo.Config;
using WebApiDemo.Common;

namespace WebApiDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cr = new ContentResult
            {
                Content = string.Format("Api Version {0}", MyConfig.Version),
                ContentEncoding = Encoding.UTF8,
                ContentType = "text/html"
            };

            return cr;
        }
    }
}