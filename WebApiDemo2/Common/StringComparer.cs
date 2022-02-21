using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebApiDemo2.Common;

namespace WebApiDemo2.Common
{
    /// <summary>
    /// String First Char Comparer
    /// </summary>
    public class StringFirstCharComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return CommonTool.CompareStringChar(x, y, 0);
        }
    }

    /// <summary>
    /// String Full Char Comparer
    /// </summary>
    public class StringFullCharComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int result = 0;

            int i = 0;
            while (true)
            {
                result = CommonTool.CompareStringChar(x, y, i);
                if (result == 0)
                {
                    if (x.Length > (i + 1) || y.Length > (i + 1))
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}