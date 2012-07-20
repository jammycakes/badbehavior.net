namespace BadBehavior.Logging
{
    /* ====== NullLogger class ====== */

    /// <summary>
    ///  Discards all log entries, providing no option to review the logs.
    /// </summary>

    public class NullLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
        }

        public void Clear()
        {
        }

        public LogResultSet Query(LogQuery criteria)
        {
            return null;
        }
    }
}
