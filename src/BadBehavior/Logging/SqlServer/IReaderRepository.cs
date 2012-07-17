using System;

namespace BadBehavior.Logging.SqlServer
{
    public interface IReaderRepository
    {
        int Count(BadBehavior.Logging.LogQuery query);
        System.Collections.Generic.IEnumerable<BadBehavior.Logging.LogEntry> GetLogEntries(BadBehavior.Logging.LogQuery query);
    }
}
