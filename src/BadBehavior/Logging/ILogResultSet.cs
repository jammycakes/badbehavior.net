using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    public interface ILogResultSet
    {
        /// <summary>
        ///  Get the current page number of the query.
        /// </summary>

        int Page { get; }

        /// <summary>
        ///  Get the total number of pages available.
        /// </summary>

        int TotalPages { get; }

        /// <summary>
        ///  Get the log entries returned by the query.
        /// </summary>
        /// <returns></returns>

        IEnumerable<LogEntry> GetLogEntries();
    }
}
