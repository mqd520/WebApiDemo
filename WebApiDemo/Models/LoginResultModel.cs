using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    /// <summary>
    /// Login Result Model
    /// </summary>
    public class LoginResultModel
    {
        public int Result { get; set; }

        /// <summary>
        /// JWT Token
        /// </summary>
        public string JWTToken { get; set; }
    }
}