﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Common;

using WebApiDemo.Common;
using WebApiDemo.Models;

namespace WebApiDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpApplication http = sender as HttpApplication;
            Exception exception = Server.GetLastError();
            Exception[] exceptions = http.Context.AllErrors;
            CommonLogger.WriteLog(
                ELogCategory.Fatal,
                string.Format("WebApiApplication Application_Error: {0}", exception.Message),
                exception
            );

#if !DEBUG
            Server.ClearError();
            http.Response.StatusCode = 500;
#endif
        }
    }
}