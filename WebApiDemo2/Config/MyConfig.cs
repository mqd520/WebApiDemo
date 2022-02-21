using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebApiDemo2.Config
{
    /// <summary>
    /// MyConfig
    /// </summary>
    public static class MyConfig
    {
        static MyConfig()
        {
            var nvc = ConfigurationManager.AppSettings;

            SignKey = nvc["SignKey"];

            {
                int n = 0;
                bool b = int.TryParse(nvc["AccessExpireTime"], out n);
                if (b)
                {
                    AccessExpireTime = n;
                }
            }
        }

        /// <summary>
        /// Get SignKey
        /// </summary>
        public static string SignKey { get; private set; }

        /// <summary>
        /// Get AccessTimeout, Unit: second
        /// </summary>
        public static int AccessExpireTime { get; private set; } = 10;
    }
}