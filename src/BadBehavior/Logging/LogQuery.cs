using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadBehavior.Logging
{
    /// <summary>
    ///  Contains the criteria by which we are to filter, sort and page through the logs.
    /// </summary>

    public class LogQuery
    {
        private string _filter;
        private string _sort;

        private static readonly Regex reCheckValue = new Regex(@"^[A-Za-z0-9_]+$", RegexOptions.Singleline);

        private static void CheckValue(string value)
        {
            if (!String.IsNullOrEmpty(value) && !reCheckValue.IsMatch(value))
                throw new ArgumentException("Not a valid value");
        }


        /// <summary>
        ///  The number of the page to return.
        /// </summary>

        public int PageNumber { get; set; }

        /// <summary>
        ///  The number of records to return on each page.
        /// </summary>

        public int PageSize { get; set; }

        /// <summary>
        ///  The name of the column on which to filter, or null if none.
        /// </summary>

        public string Filter {
            get { return _filter; }
            set
            {
                CheckValue(value);
                _filter = value;
            }
        }

        /// <summary>
        ///  The value of the column to be filtered, or null if none.
        /// </summary>

        public string FilterValue { get; set; }

        /// <summary>
        ///  The column to sort on, or null for the default.
        /// </summary>

        public string Sort
        {
            get { return _sort; }
            set
            {
                CheckValue(value);
                _sort = value;
            }
        }

        /// <summary>
        ///  true to sort in ascending order, otherwise descending.
        /// </summary>

        public bool Ascending { get; set; }
    }
}
