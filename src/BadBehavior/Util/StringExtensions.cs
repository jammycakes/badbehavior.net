using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadBehavior.Util
{
    public static class StringExtensions
    {
        private static readonly Regex reCheckValue = new Regex(@"^[A-Za-z0-9_]+$", RegexOptions.Singleline);

        /// <summary>
        ///  Asserts that user input is safe for string concatenation in a query.
        /// </summary>
        /// <param name="value"></param>

        public static void AssertSafe(this string value)
        {
            if (!String.IsNullOrEmpty(value) && !reCheckValue.IsMatch(value))
                throw new ArgumentException("Not a valid value");
        }
    }
}
