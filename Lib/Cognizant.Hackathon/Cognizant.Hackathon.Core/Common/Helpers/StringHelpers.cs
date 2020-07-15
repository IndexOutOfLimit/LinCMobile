using System.Diagnostics;
using System.Web;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

   
    public static class StringHelpers
    {
        #region Static Fields        
        private static readonly char[] hexChars = "0123456789abcdef".ToCharArray();

        #endregion

        public static string ReplaceLast(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1) return source;

            return source.Remove(place, find.Length).Insert(place, replace);
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(str, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
        }
        
    }
    
}