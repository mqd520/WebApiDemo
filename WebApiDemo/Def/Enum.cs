using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Def
{
    #region Common
    /// <summary>
    /// Common Result Code
    /// </summary>
    public enum ECommonResultCode
    {
        Success = 0,

        Failure = 100
    }
    #endregion


    /// <summary>
    /// Login Result Code
    /// </summary>
    public enum ELoginResultCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 0,

        /// <summary>
        /// User or Pwd Error
        /// </summary>
        UserPwdError = 101,

        /// <summary>
        /// Code Error
        /// </summary>
        CodeError = 102
    }
}