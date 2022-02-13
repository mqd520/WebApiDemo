using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

using Common;

using WebApiDemo.Def;
using WebApiDemo.Config;
using WebApiDemo.Models;
using WebApiDemo.Common;

namespace WebApiDemo.Filters
{
    public class CustomAuthorizeFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool bIsAuthorized = false;
            string message = null;

            try
            {
                var item = actionContext.Request.Headers.Where(x => x.Key.Equals(MyConfig.JWTHeader)).FirstOrDefault();
                if (!string.IsNullOrEmpty(item.Key))
                {
                    var arr = item.Value.ToArray();
                    if (arr != null && arr.Length > 0)
                    {
                        string token = arr[0];
                        if (!string.IsNullOrEmpty(token))
                        {
                            var authInfo = JWTTool.ParseToken<JWTAuthInfoModel>(token);
                            var timestamp = DateTime.Now.GetTimestamp();
                            if (authInfo != null && timestamp < authInfo.ExpireTime)
                            {
                                actionContext.RequestContext.RouteData.Values.Add(MyConsts.AuthInfoKey, authInfo);

                                bIsAuthorized = true;
                            }
                            else
                            {
                                message = string.Format("Invalid Authorization Info");
                            }
                        }
                        else
                        {
                            message = string.Format("Http Header({0}) Value Is Empty", MyConfig.JWTHeader);
                        }
                    }
                    else
                    {
                        message = string.Format("Http Header({0}) Value Is Empty", MyConfig.JWTHeader);
                    }
                }
                else
                {
                    message = string.Format("Not Found Http Header: {0}", MyConfig.JWTHeader);
                }
            }
            catch (Exception ex)
            {
                CommonLogger.WriteLog(
                    ELogCategory.Fatal,
                    string.Format("CustomAuthorizeFilterAttribute.IsAuthorized Exception: {0}", ex.Message),
                    ex
                );
            }

            if (!bIsAuthorized)
            {
                CommonLogger.WriteLog(
                    ELogCategory.Warn,
                    string.Format("Invalid JWT: {0}", message)
                );
            }

            return bIsAuthorized;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}