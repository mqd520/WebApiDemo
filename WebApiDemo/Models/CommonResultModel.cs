using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    /// <summary>
    /// Common Result Model
    /// </summary>
    public class CommonResultModel
    {
        public int Code { get; set; }

        public string Data { get; set; }
    }
}