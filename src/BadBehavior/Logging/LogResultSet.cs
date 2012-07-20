using System;
using System.Collections.Generic;

namespace BadBehavior.Logging
{
    /* ====== LogResultSet class ====== */

    /// <summary>
    ///  The result of a query to an <see cref="ILogger"/> instance.
    /// </summary>

    public class LogResultSet
    {
        /// <summary>
        ///  Get the current page number of the query.
        /// </summary>

        public int Page { get; set; }

        /// <summary>
        ///  Get the total number of pages available.
        /// </summary>

        public int TotalPages { get; set; }

        /// <summary>
        ///  Get the number of records available on each page.
        /// </summary>

        public int PageSize { get; set; }

        /// <summary>
        ///  Get the total number of entries available in this query.
        /// </summary>

        public int TotalEntries { get; set; }

        /// <summary>
        ///  Get the log entries returned by the query.
        /// </summary>
        /// <returns></returns>

        public IList<LogEntry> LogEntries { get; set; }


        /* ====== Computed properties ======= */

        /// <summary>
        ///  The number of the first entry in the result set.
        /// </summary>

        public int FirstEntry
        {
            get
            {
                return (Page - 1) * PageSize + 1;
            }
        }

        /// <summary>
        ///  The number of the last entry in the result set.
        /// </summary>

        public int LastEntry
        {
            get
            {
                return Math.Min(Page * PageSize, TotalEntries);
            }
        }
    }
}
