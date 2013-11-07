using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HL7_TCP.Web
{
    public static class StringExtensions
    {

        public static string FormatWith(this string input, params object[] formatArgs)
        {
            if (input == null)
                throw new ArgumentNullException("format");
            if (formatArgs == null)
                throw new ArgumentNullException("formatArgs");

            return string.Format(input, formatArgs);
        }

        public static int? ToNullableInt(this string input) 
        {
            if (input != string.Empty)
                return int.Parse(input);
            else
                return (int?)null;
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}