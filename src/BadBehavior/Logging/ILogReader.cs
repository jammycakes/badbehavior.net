using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    public interface ILogReader
    {
        /// <summary>
        ///  Reads a number of events from the log.
        /// </summary>
        /// <param name="skip">
        ///  The number of events to skip.
        /// </param>
        /// <param name="take">
        ///  The number of events to take.
        /// </param>

        IEnumerable<LogEntry> Read(int skip, int take);

        /// <summary>
        ///  Reads all events from the log.
        /// </summary>
        /// <returns></returns>

        IEnumerable<LogEntry> ReadAll();

        /// <summary>
        ///  Reads all events between a given start and end time.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>

        IEnumerable<LogEntry> Read(DateTime start, DateTime end);

        /// <summary>
        ///  Returns the total number of entries in the log.
        /// </summary>
        /// <returns></returns>

        long Count();

        /// <summary>
        ///  Returns the number of entries in the log on a specific date range.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>

        long Count(DateTime start, DateTime end);
    }
}
