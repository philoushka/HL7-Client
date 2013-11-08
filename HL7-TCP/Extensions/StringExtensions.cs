using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7_TCP.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string input)
        {
            return (input.IsNullOrEmpty() == false);
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
        public static string FormatWith(this string input, params object[] formatArgs)
        {
            if (input == null)
                throw new ArgumentNullException("format");
            if (formatArgs == null)
                throw new ArgumentNullException("formatArgs");

            return string.Format(input, formatArgs);
        }
    }
}
