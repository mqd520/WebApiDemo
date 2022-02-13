using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Newtonsoft.Json;

using WebApiDemo.Filters;

namespace WebApiDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //config.Filters.Add(new CustomAuthorizeFilterAttribute());


            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            json.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            json.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

            var xml = config.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;
        }
    }
}
