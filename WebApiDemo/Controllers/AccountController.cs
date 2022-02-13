using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Common;

using WebApiDemo.Def;
using WebApiDemo.Models;
using WebApiDemo.Filters;
using WebApiDemo.Common;

namespace WebApiDemo.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public LoginResultModel Login([FromBody] LoginInfoModel model)
        {
            var result = new LoginResultModel
            {
                Result = (int)ELoginResultCode.Success,
                JWTToken = JWTTool.CreateToken(new JWTAuthInfoModel
                {
                    ExpireTime = DateTime.Now.AddMinutes(20).GetTimestamp(),
                    Role = "",
                    UserName = "AAtest001"
                })
            };

            return result;
        }

        [HttpGet]
        public CommonResultModel Logout()
        {
            return new Models.CommonResultModel
            {
                Code = 0
            };
        }

        protected override void Dispose(bool disposing)
        {
            // ...
            base.Dispose(disposing);
        }
    }
}
