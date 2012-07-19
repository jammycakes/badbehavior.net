namespace BadBehavior.Logging
{
    /* ====== ILogger interface ====== */

    /// <summary>
    ///  Represents a backing store for Bad Behavior violations.
    /// </summary>

    public interface ILogger
    {
        /// <summary>
        ///  Adds a new entry to the logs.
        /// </summary>
        /// <param name="entry">
        ///  The <see cref="LogEntry"/> instance to log.
        /// </param>

        void Log(LogEntry entry);

        /// <summary>
        ///  Clears all entries from the logs.
        /// </summary>

        void Clear();

        /// <summary>
        ///  Queries the logs for a page of records.
        /// </summary>
        /// <param name="criteria">
        ///  A <see cref="LogQuery"/> object containing the criteria on which we wish
        ///  to sort, filter and page.
        /// </param>
        /// <returns>
        ///  A query result, or null if querying is not available.
        /// </returns>

        LogResultSet Query(LogQuery criteria);
    }
}
