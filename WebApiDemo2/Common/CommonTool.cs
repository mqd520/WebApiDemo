using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo2.Common
{
    /// <summary>
    /// Common Tool
    /// </summary>
    public static class CommonTool
    {
        static CommonTool()
        {

        }

        /// <summary>
        /// Compare String Char
        /// </summary>
        /// <param name="x">String X</param>
        /// <param name="y">String Y</param>
        /// <param name="i">Index</param>
        /// <returns></returns>
        public static int CompareStringChar(string x, string y, int i)
        {
            if (i < 0)
            {
                throw new Exception(string.Format("CompareStringLetter Parameter Invalid: x = {0}, y = {1}, i = {2}", x, y, i));
            }

            int ascIIX = -1;
            int ascIIY = -1;

            if (x.Length >= (i + 1))
            {
                ascIIX = (int)x.ElementAt(i);
            }
            if (y.Length >= (i + 1))
            {
                ascIIY = (int)y.ElementAt(i);
            }

            if (ascIIX == ascIIY)
            {
                return 0;
            }
            else if (ascIIX < ascIIY)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}