using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo2.Def
{
    /// <summary>
    /// Check Parameter Result Code
    /// </summary>
    public enum ECheckParameterResultCode
    {
        Success = 0,

        PartnerInvalid = 101,

        Expire = 102,

        SignInvalid = 103,

        None = 999
    }
}