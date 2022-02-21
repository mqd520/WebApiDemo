using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft;
using Newtonsoft.Json;

using WebApiDemo.Def;
using WebApiDemo.Models;

namespace WebApiDemo.Common
{
    /// <summary>
    /// Common Result Data Tool
    /// </summary>
    public static class CommonResultModelTool
    {
        static CommonResultModelTool()
        {

        }

        public static CommonResultModel Create(string data, ECommonResultCode code = ECommonResultCode.Success)
        {
            return new CommonResultModel
            {
                Code = (int)code,
                Data = data
            };
        }

        public static CommonResultModel Create<T>(T data, ECommonResultCode code = ECommonResultCode.Success)
        {
            string json = JsonConvert.SerializeObject(data);
            return Create(json, code);
        }

        public static void Write2Response(HttpResponse response, string data, ECommonResultCode code = ECommonResultCode.Success)
        {
            var model = Create(data, code);
            string content = JsonConvert.SerializeObject(model);

            response.ClearContent();
            response.Write(content);
        }

        public static void Write2Response<T>(HttpResponse response, T data, ECommonResultCode code = ECommonResultCode.Success)
        {
            string json = JsonConvert.SerializeObject(data);
            Write2Response(response, json, code);
        }
    }
}