namespace BadBehavior.Logging
{
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
