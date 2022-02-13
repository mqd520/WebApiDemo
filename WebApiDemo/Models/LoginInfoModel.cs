using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    /// <summary>
    /// Login Info Model
    /// </summary>
    public class LoginInfoModel
    {
        public string UserName { get; set; }

        public string Pwd { get; set; }

        public string Code { get; set; }
    }
}