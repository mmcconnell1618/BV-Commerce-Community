using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web.Css
{
    public class Compressor
    {
        public static string Minify(string input)
        {
            string result = input;

            if (!string.IsNullOrEmpty(input))
            {
                result = Yahoo.Yui.Compressor.CssCompressor.Compress(input);
            }

            return result;
        }
    }
}
