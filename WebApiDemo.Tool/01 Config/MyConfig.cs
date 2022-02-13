using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WebApiDemo.Tool._01_Config
{
    /// <summary>
    /// MyConfig
    /// </summary>
    public static class MyConfig
    {
        static MyConfig()
        {

        }

        public static string ApiServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiServer"];
            }
        }
    }
}
