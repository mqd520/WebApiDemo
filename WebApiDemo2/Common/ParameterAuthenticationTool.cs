using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Common;

using WebApiDemo2.Def;
using WebApiDemo2.Config;

namespace WebApiDemo2.Common
{
    /// <summary>
    /// Parameter Authentication Tool
    /// </summary>
    public static class ParameterAuthenticationTool
    {
        static ParameterAuthenticationTool()
        {

        }

        /// <summary>
        /// Check
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static ECheckParameterResultCode Check(Dictionary<string, string> dict)
        {
            #region Check Sign
            {
                var dict2 = dict.Where(x => !string.IsNullOrEmpty(x.Value) && !x.Key.Equals("sign"))
                                .OrderBy(x => x.Key, new StringFullCharComparer());
                string str = dict2.ConcatElement("&", x => string.Format("{0}={1}", x.Key, x.Value));
                string str2 = str + MyConfig.SignKey;
                string cipherText = Md5EncryptionTool.Encrypt(str);

                bool b = false;
                if (dict.ContainsKey("sign"))
                {
                    var sign = dict["sign"];
                    if (sign.Equals(cipherText))
                    {
                        b = true;
                    }
                }

                if (!b)
                {
                    return ECheckParameterResultCode.SignInvalid;
                }
            }
            #endregion


            #region Check Expire
            {
                bool b = false;

                if (dict.ContainsKey("t"))
                {
                    int timestamp = 0;
                    bool b2 = int.TryParse(dict["t"], out timestamp);
                    if (b2)
                    {
                        int timestamp2 = DateTime.Now.GetTimestamp();
                        if (timestamp2 > timestamp && timestamp2 - timestamp <= MyConfig.AccessExpireTime)
                        {
                            b = true;
                        }
                    }
                }

                if (!b)
                {
                    return ECheckParameterResultCode.Expire;
                }
            }
            #endregion

            return ECheckParameterResultCode.None;
        }
    }
}