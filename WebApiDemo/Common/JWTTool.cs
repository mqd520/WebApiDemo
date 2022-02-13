using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;

using JWT;
using JWT.Algorithms;
using JWT.Serializers;

using WebApiDemo.Config;

namespace WebApiDemo.Common
{
    /// <summary>
    /// JWT Tool
    /// </summary>
    public static class JWTTool
    {
        static JWTTool()
        {

        }

        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="obj">Payload Obj</param>
        /// <returns></returns>
        public static string CreateToken(string key, object obj)
        {
            byte[] buf = Encoding.UTF8.GetBytes(key);
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            string token = encoder.Encode(obj, key);

            return token;
        }

        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="obj">Payload Obj</param>
        /// <returns></returns>
        public static string CreateToken(object obj)
        {
            return CreateToken(MyConfig.JWTKey, obj);
        }

        /// <summary>
        /// Parse Token
        /// </summary>
        /// <typeparam name="T">Authorization Info</typeparam>
        /// <param name="key">Key</param>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public static T ParseToken<T>(string key, string token)
        {
            byte[] buf = Encoding.UTF8.GetBytes(key);
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
            var authInfo = decoder.DecodeToObject<T>(token, key, verify: true);

            return authInfo;
        }

        /// <summary>
        /// Parse Token
        /// </summary>
        /// <typeparam name="T">Authorization Info</typeparam>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public static T ParseToken<T>(string token)
        {
            return ParseToken<T>(MyConfig.JWTKey, token);
        }
    }
}