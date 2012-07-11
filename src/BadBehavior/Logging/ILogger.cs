using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    public interface ILogger
    {
        /// <summary>
        ///  Adds a new entry to the logs.
        /// </summary>
        /// <param name="entry"></param>

        void Log(LogEntry entry);

        /// <summary>
        ///  Clears all entries from the logs.
        /// </summary>

        void Clear();

        /// <summary>
        ///  Queries the logs for a page of records.
        /// </summary>
        /// <param name="page">
        ///  The page number to return.
        /// </param>
        /// <param name="pageSize">
        ///  The number of records to return on each page.
        /// </param>
        /// <param name="filter">
        ///  The filter to apply to the data set.
        /// </param>
        /// <returns>
        ///  A query result, or null if querying is not available.
        /// </returns>

        ILogQuery Query(int page, int pageSize, string filter);
    }
}
