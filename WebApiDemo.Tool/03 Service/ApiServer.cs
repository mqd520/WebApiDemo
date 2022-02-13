using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Newtonsoft;
using Newtonsoft.Json;

using Common;

using WebApiDemo.Tool._00_Def;

namespace WebApiDemo.Tool._03_Service
{
    /// <summary>
    /// Api Server
    /// </summary>
    public static class ApiServer
    {
        #region Properties
        public static string ApiServerAddr { get; private set; }

        public static string LoginPath { get; private set; } = "api/Account/Login";
        public static string LoginUrl { get { return GetFullUrl(ApiServerAddr, LoginPath); } }
        #endregion


        static ApiServer()
        {
            ApiServerAddr = ConfigurationManager.AppSettings["ApiServerAddr"];
        }

        private static string GetFullUrl(string addr, string path)
        {
            if (string.IsNullOrEmpty(addr))
            {
                return path;
            }

            string strPrefix = "";
            if (addr.EndsWith("/"))
            {
                strPrefix = addr.Substring(0, addr.Length - 1);
            }
            else
            {
                strPrefix = addr;
            }

            string strSuffix = "";
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("/"))
                {
                    strSuffix = path;
                }
                else
                {
                    strSuffix = "/" + path;
                }
            }
            else
            {
                strSuffix = "/";
            }

            string str = strPrefix + strSuffix;

            return str;
        }

        private async static Task<string> SendJsonAsync<T>(string url, T model)
        {
            string json = JsonConvert.SerializeObject(model);
            return await HttpClientTool.SendJsonAsync(url, json, Encoding.UTF8, null);
        }

        public async static Task<string> LoginAsync(LoginInfoModel model)
        {
            return await SendJsonAsync(LoginUrl, model);
        }
    }
}
