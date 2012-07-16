using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    /// <summary>
    ///  Contains the criteria by which we are to filter, sort and page through the logs.
    /// </summary>

    public class LogQuery
    {
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

        public string Filter { get; set; }

        /// <summary>
        ///  The value of the column to be filtered, or null if none.
        /// </summary>

        public string FilterValue { get; set; }

        /// <summary>
        ///  The column to sort on, or null for the default.
        /// </summary>

        public string Sort { get; set; }

        /// <summary>
        ///  true to sort in ascending order, otherwise descending.
        /// </summary>

        public bool Ascending { get; set; }
    }
}
