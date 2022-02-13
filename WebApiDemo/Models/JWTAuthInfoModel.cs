using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    /// <summary>
    /// User Authorization Info Model
    /// </summary>
    public class JWTAuthInfoModel
    {
        public string UserName { get; set; }

        public string Role { get; set; }

        public int ExpireTime { get; set; }
    }
}