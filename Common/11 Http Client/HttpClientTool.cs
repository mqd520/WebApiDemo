using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Common
{
    /// <summary>
    /// Http Client Tool
    /// </summary>
    public static class HttpClientTool
    {
        /// <summary>
        /// Get or Set UserAgent
        /// </summary>
        public static string UserAgent { get; set; }


        static HttpClientTool()
        {
            UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
        }


        #region Common
        /// <summary>
        /// Send Http Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <param name="bodyData"></param>
        /// <param name="reqAfter"></param>
        /// <param name="reqBefore"></param>
        public static void SendHttpReq(
            string url,
            string method,
            Dictionary<string, string> headers,
            string contentType, byte[] bodyData,
            Action<HttpWebRequest, WebResponse> reqAfter = null,
            Action<HttpWebRequest> reqBefore = null)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            hwr.ContentType = contentType;
            hwr.UserAgent = UserAgent;
            hwr.Method = method;
            hwr.ServicePoint.ConnectionLimit = int.MaxValue;

            if (headers != null && headers.Count > 0)
            {
                foreach (var item in headers)
                {
                    if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                    {
                        hwr.Headers.Add(item.Key, item.Value);
                    }
                }
            }

            hwr.ContentLength = 0;
            if (bodyData != null && bodyData.Length > 0)
            {
                hwr.ContentLength = bodyData.Length;
                hwr.GetRequestStream().Write(bodyData, 0, bodyData.Length);
            }

            if (reqBefore != null)
            {
                reqBefore.Invoke(hwr);
            }

            WebResponse response = hwr.GetResponse();

            if (reqAfter != null)
            {
                reqAfter.Invoke(hwr, response);
            }
        }

        /// <summary>
        /// Send Http Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <param name="bodyData"></param>
        /// <param name="actionComplete"></param>
        /// <param name="actionBefore"></param>
        /// <returns></returns>
        public async static Task SendHttpReqAsync(
            string url,
            string method,
            Dictionary<string, string> headers,
            string contentType, byte[] bodyData,
            Action<HttpWebRequest, WebResponse> actionComplete = null,
            Action<HttpWebRequest> actionBefore = null)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            hwr.ContentType = contentType;
            hwr.UserAgent = UserAgent;
            hwr.Method = method;
            hwr.ServicePoint.ConnectionLimit = int.MaxValue;

            if (headers != null && headers.Count > 0)
            {
                foreach (var item in headers)
                {
                    if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                    {
                        hwr.Headers.Add(item.Key, item.Value);
                    }
                }
            }

            if (bodyData != null && bodyData.Length > 0)
            {
                hwr.ContentLength = bodyData.Length;
                var s = hwr.GetRequestStream();
                await s.WriteAsync(bodyData, 0, bodyData.Length);
            }

            if (actionBefore != null)
            {
                actionBefore.Invoke(hwr);
            }

            var response = await hwr.GetResponseAsync();

            if (actionComplete != null)
            {
                actionComplete.Invoke(hwr, response);
            }
        }


        /// <summary>
        /// ParseBuffer
        /// </summary>
        /// <param name="response"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] ParseBuffer(HttpWebResponse response, Stream s)
        {
            byte[] buf = new byte[0];

            if (response.ContentLength >= 0)
            {
                buf = new byte[response.ContentLength];
                s.Read(buf, 0, buf.Length);
            }
            else
            {
                if (response.ContentLength == -1)
                {
                    IList<byte[]> ls = new List<byte[]>();
                    int count = 0;
                    while (s.CanRead)
                    {
                        byte[] buf1 = new byte[1024];
                        int len = s.Read(buf1, 0, buf1.Length);
                        if (len > 0)
                        {
                            ls.Add(buf1);
                            count += len;
                        }

                        if (len < 1024)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(100);
                    }

                    buf = new byte[count];
                    int index = 0;
                    foreach (var item in ls)
                    {
                        int len = Math.Min(item.Length, count - index);
                        Array.Copy(item, 0, buf, index, len);
                        index += item.Length;
                    }
                }
            }

            return buf;
        }

        /// <summary>
        /// Parse Buffer
        /// </summary>
        /// <param name="response"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public async static Task<byte[]> ParseBufferAsync(HttpWebResponse response, Stream s)
        {
            byte[] buf = new byte[0];

            if (response.ContentLength >= 0)
            {
                buf = new byte[response.ContentLength];
                await s.ReadAsync(buf, 0, buf.Length);
            }
            else
            {
                if (response.ContentLength == -1)
                {
                    IList<byte[]> ls = new List<byte[]>();
                    int count = 0;
                    while (s.CanRead)
                    {
                        byte[] buf1 = new byte[1024];
                        int len = await s.ReadAsync(buf1, 0, buf1.Length);
                        if (len > 0)
                        {
                            ls.Add(buf1);
                            count += len;
                        }

                        if (len < 1024)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(50);
                    }

                    buf = new byte[count];
                    int index = 0;
                    foreach (var item in ls)
                    {
                        int len = Math.Min(item.Length, count - index);
                        Array.Copy(item, 0, buf, index, len);
                        index += item.Length;
                    }
                }
            }

            return buf;
        }
        #endregion


        #region Send Form
        /// <summary>
        /// Build Form Data
        /// </summary>
        /// <param name="form">form</param>
        /// <param name="encode">encode</param>
        /// <returns>string</returns>
        private static string BuildFormData(Dictionary<string, string> form, Encoding encode)
        {
            string postData = "";

            if (form != null && form.Count > 0)
            {
                foreach (var item in form)
                {
                    postData += string.Format("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value));
                }
            }

            if (postData.Length > 0)
            {
                postData = postData.TrimEnd('&');
            }

            return postData;
        }

        /// <summary>
        /// Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <param name="encode">encode</param>
        /// <param name="headers">dictHeaders</param>
        /// <param name="lsResponseCookie">lsResponseCookie</param>
        /// <returns>string</returns>
        public static string SendForm(string url, Dictionary<string, string> form, Encoding encode, Dictionary<string, string> headers, IList<HttpCookie> lsResponseCookie)
        {
            string result = "";

            string str = BuildFormData(form, encode);
            byte[] bytes = string.IsNullOrEmpty(str) ? new byte[0] : encode.GetBytes(str);
            SendHttpReq(url, "POST", headers, "application/x-www-form-urlencoded", bytes, (x, y) =>
            {
                if (lsResponseCookie != null)
                {
                    var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                    foreach (var item in ls)
                    {
                        lsResponseCookie.Add(item);
                    }
                }

                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            });

            return result;
        }

        /// <summary>
        /// Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <param name="headers">dictHeaders</param>
        /// <param name="lsResponseCookie">lsResponseCookie</param>
        /// <returns>string</returns>
        public static string SendForm(string url, Dictionary<string, string> form, Dictionary<string, string> headers, IList<HttpCookie> lsResponseCookie)
        {
            return SendForm(url, form, Encoding.UTF8, headers, lsResponseCookie);
        }

        /// <summary>
        /// Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <param name="headers">dictHeaders</param>
        /// <returns>string</returns>
        public static string SendForm(string url, Dictionary<string, string> form, Dictionary<string, string> headers)
        {
            return SendForm(url, form, Encoding.UTF8, headers, null);
        }

        /// <summary>
        /// Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <returns>string</returns>
        public static string SendForm(string url, Dictionary<string, string> form)
        {
            return SendForm(url, form, Encoding.UTF8, null, null);
        }

        /// <summary>
        /// Binary Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <param name="encode">encode</param>
        /// <param name="headers">dictHeaders</param>
        /// <returns>string</returns>
        public static byte[] BinarySendForm(string url, Dictionary<string, string> form, Encoding encode, Dictionary<string, string> headers)
        {
            byte[] buf = new byte[0];

            string str = BuildFormData(form, encode);
            byte[] bytes = string.IsNullOrEmpty(str) ? new byte[0] : encode.GetBytes(str);
            SendHttpReq(url, "POST", headers, "application/x-www-form-urlencoded", bytes, (x, y) =>
            {
                buf = new byte[y.ContentLength];
                var s = y.GetResponseStream();
                s.Read(buf, 0, buf.Length);
            });

            return buf;
        }

        /// <summary>
        /// Binary Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <param name="headers">dictHeaders</param>
        /// <returns>string</returns>
        public static byte[] BinarySendForm(string url, Dictionary<string, string> form, Dictionary<string, string> headers)
        {
            return BinarySendForm(url, form, Encoding.UTF8, headers);
        }

        /// <summary>
        /// Binary Send Form
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="form">form</param>
        /// <returns>string</returns>
        public static byte[] BinarySendForm(string url, Dictionary<string, string> form)
        {
            return BinarySendForm(url, form, Encoding.UTF8, null);
        }
        #endregion


        #region Send Json
        #region Send Json Sync
        /// <summary>
        /// Send Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="encode"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string SendJson(string url, string json, Encoding encode, Dictionary<string, string> headers)
        {
            string result = "";

            byte[] bytes = string.IsNullOrEmpty(json) ? new byte[0] : encode.GetBytes(json);
            SendHttpReq(url, "POST", headers, "application/json", bytes, (x, y) =>
            {
                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            });

            return result;
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string SendJson(string url, string json, Dictionary<string, string> headers)
        {
            return SendJson(url, json, Encoding.UTF8, headers);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string SendJson(string url, string json, string cookie)
        {
            Dictionary<string, string> header = null;
            if (!string.IsNullOrEmpty(cookie))
            {
                header = new Dictionary<string, string>();
                header.Add("Cookie", cookie);
            }

            return SendJson(url, json, Encoding.UTF8, header);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="encode"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string SendJson<T>(string url, T obj, Encoding encode, Dictionary<string, string> headers)
        {
            string json = "";
            if (obj != null)
            {
                json = JsonConvert.SerializeObject(obj);
            }

            return SendJson(url, json, encode, headers);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string SendJson<T>(string url, T obj, Dictionary<string, string> headers)
        {
            return SendJson<T>(url, obj, Encoding.UTF8, headers);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SendJson<T>(string url, T obj)
        {
            return SendJson<T>(url, obj, Encoding.UTF8, null);
        }

        public static string SendJson<T>(string url, T obj, string cookie)
        {
            var dict = new Dictionary<string, string>();
            dict["Cookie"] = cookie;

            return SendJson<T>(url, obj, Encoding.UTF8, dict);
        }
        #endregion


        #region Send Json Async
        /// <summary>
        /// Send Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="encode"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async static Task<string> SendJsonAsync(string url, string json, Encoding encode, Dictionary<string, string> headers)
        {
            string result = "";

            byte[] bytes = string.IsNullOrEmpty(json) ? new byte[0] : encode.GetBytes(json);
            await SendHttpReqAsync(url, "POST", headers, "application/json", bytes, async (x, y) =>
            {
                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = await sr.ReadToEndAsync();
                }
            });

            return result;
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async static Task<string> SendJsonAsync(string url, string json, Dictionary<string, string> headers)
        {
            return await SendJsonAsync(url, json, Encoding.UTF8, headers);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<string> SendJsonAsync(string url, string json, string cookie)
        {
            Dictionary<string, string> header = null;
            if (!string.IsNullOrEmpty(cookie))
            {
                header = new Dictionary<string, string>();
                header.Add("Cookie", cookie);
            }

            return await SendJsonAsync(url, json, Encoding.UTF8, header);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="encode"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async static Task<string> SendJsonAsync<T>(string url, T obj, Encoding encode, Dictionary<string, string> headers)
        {
            string json = "";
            if (obj != null)
            {
                json = JsonConvert.SerializeObject(obj);
            }

            return await SendJsonAsync(url, json, encode, headers);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async static Task<string> SendJsonAsync<T>(string url, T obj, Dictionary<string, string> headers)
        {
            return await SendJsonAsync<T>(url, obj, Encoding.UTF8, headers);
        }

        /// <summary>
        /// Send Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async static Task<string> SendJsonAsync<T>(string url, T obj)
        {
            return await SendJsonAsync<T>(url, obj, Encoding.UTF8, null);
        }

        public async static Task<string> SendJsonAsync<T>(string url, T obj, string cookie)
        {
            var dict = new Dictionary<string, string>();
            dict["Cookie"] = cookie;

            return await SendJsonAsync<T>(url, obj, Encoding.UTF8, dict);
        }
        #endregion
        #endregion


        #region Send Get
        #region Send Get Sync
        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string SendGet(string url, string cookie)
        {
            string result = "";

            Dictionary<string, string> header = null;
            if (!string.IsNullOrEmpty(cookie))
            {
                header = new Dictionary<string, string>();
                header.Add("Cookie", cookie);
            }

            SendHttpReq(url, "GET", header, "text/plain", null, (x, y) =>
            {
                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            });

            return result;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendGet(string url)
        {
            return SendGet(url, string.Empty);
        }

        public static string SendGet(string url, IList<HttpCookie> lsResCookie)
        {
            string result = "";

            SendHttpReq(url, "GET", null, "text/plain", null, (x, y) =>
            {
                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

                var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                foreach (var item in ls)
                {
                    lsResCookie.Add(item);
                }
            });

            return result;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] BinarySendGet(string url)
        {
            byte[] buf = new byte[0];

            SendHttpReq(url, "GET", null, "text/plain", null, (x, y) =>
            {
                buf = ParseBuffer((HttpWebResponse)y, y.GetResponseStream());
            });

            return buf;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="lsResCookie"></param>
        /// <returns></returns>
        public static byte[] BinarySendGet(string url, IList<HttpCookie> lsResCookie)
        {
            byte[] buf = new byte[0];

            SendHttpReq(url, "GET", null, "text/plain", null, (x, y) =>
            {
                buf = ParseBuffer((HttpWebResponse)y, y.GetResponseStream());

                var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                foreach (var item in ls)
                {
                    lsResCookie.Add(item);
                }
            });

            return buf;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="lsResCookie"></param>
        /// <returns></returns>
        public static byte[] BinarySendGet(string url, string cookie, IList<HttpCookie> lsResCookie)
        {
            byte[] buf = new byte[0];

            Dictionary<string, string> header = null;
            if (!string.IsNullOrEmpty(cookie))
            {
                header = new Dictionary<string, string>();
                header.Add("Cookie", cookie);
            }

            SendHttpReq(url, "GET", header, "text/plain", null, (x, y) =>
            {
                buf = ParseBuffer((HttpWebResponse)y, y.GetResponseStream());

                var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                foreach (var item in ls)
                {
                    lsResCookie.Add(item);
                }
            });

            return buf;
        }
        #endregion

        #region Send Get Async
        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public async static Task<string> SendGetAsync(string url, string cookie)
        {
            string result = "";

            Dictionary<string, string> header = null;
            if (!string.IsNullOrEmpty(cookie))
            {
                header = new Dictionary<string, string>();
                header.Add("Cookie", cookie);
            }

            await SendHttpReqAsync(url, "GET", header, "text/plain", null, async (x, y) =>
            {
                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = await sr.ReadToEndAsync();
                }
            });

            return result;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<string> SendGetAsync(string url)
        {
            return await SendGetAsync(url, string.Empty);
        }

        public async static Task<string> SendGetAsync(string url, IList<HttpCookie> lsResCookie)
        {
            string result = "";

            await SendHttpReqAsync(url, "GET", null, "text/plain", null, async (x, y) =>
            {
                using (StreamReader sr = new StreamReader(y.GetResponseStream()))
                {
                    result = await sr.ReadToEndAsync();
                }

                var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                foreach (var item in ls)
                {
                    lsResCookie.Add(item);
                }
            });

            return result;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<byte[]> BinarySendGetAsync(string url)
        {
            byte[] buf = new byte[0];

            await SendHttpReqAsync(url, "GET", null, "text/plain", null, async (x, y) =>
            {
                buf = await ParseBufferAsync((HttpWebResponse)y, y.GetResponseStream());
            });

            return buf;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="lsResCookie"></param>
        /// <returns></returns>
        public async static Task<byte[]> BinarySendGetAsync(string url, IList<HttpCookie> lsResCookie)
        {
            byte[] buf = new byte[0];

            await SendHttpReqAsync(url, "GET", null, "text/plain", null, async (x, y) =>
            {
                buf = await ParseBufferAsync((HttpWebResponse)y, y.GetResponseStream());

                var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                foreach (var item in ls)
                {
                    lsResCookie.Add(item);
                }
            });

            return buf;
        }

        /// <summary>
        /// Send Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="lsResCookie"></param>
        /// <returns></returns>
        public async static Task<byte[]> BinarySendGetAsync(string url, string cookie, IList<HttpCookie> lsResCookie)
        {
            byte[] buf = new byte[0];

            Dictionary<string, string> header = null;
            if (!string.IsNullOrEmpty(cookie))
            {
                header = new Dictionary<string, string>();
                header.Add("Cookie", cookie);
            }

            await SendHttpReqAsync(url, "GET", header, "text/plain", null, async (x, y) =>
            {
                buf = await ParseBufferAsync((HttpWebResponse)y, y.GetResponseStream());

                var ls = CommonTool.ParseCookies(y.Headers["Set-Cookie"]);
                foreach (var item in ls)
                {
                    lsResCookie.Add(item);
                }
            });

            return buf;
        }
        #endregion
        #endregion
    }
}