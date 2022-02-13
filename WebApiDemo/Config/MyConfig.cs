using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebApiDemo.Config
{
    /// <summary>
    /// MyConfig
    /// </summary>
    public static class MyConfig
    {
        static MyConfig()
        {

        }

        /// <summary>
        /// Get Version
        /// </summary>
        public static string Version
        {
            get
            {
                return ConfigurationManager.AppSettings["Version"];
            }
        }

        /// <summary>
        /// Get JWTHeader
        /// </summary>
        public static string JWTHeader
        {
            get
            {
                return ConfigurationManager.AppSettings["JWTHeader"];
            }
        }

        /// <summary>
        /// Get JWTKey
        /// </summary>
        public static string JWTKey
        {
            get
            {
                return ConfigurationManager.AppSettings["JWTKey"];
            }
        }
    }
}