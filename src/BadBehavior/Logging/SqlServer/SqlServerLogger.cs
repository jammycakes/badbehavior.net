using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BadBehavior.Logging.SqlServer;

namespace BadBehavior.Logging.SqlServer
{
    /* ====== SqlServerLogger class ====== */

    /// <summary>
    ///  Logs Bad Behavior violations to a SQL Server database.
    /// </summary>

    public class SqlServerLogger : ILogger
    {
        /// <summary>
        ///  The <see cref="IWriterRepository"/> instance which handles writes
        ///  to SQL Server.
        /// </summary>

        public IWriterRepository Writer { get; set; }

        /// <summary>
        ///  The <see cref="IReaderRepository"/> instance which handles reads
        ///  from SQL Server.
        /// </summary>

        public IReaderRepository Reader { get; set; }

        public SqlServerLogger()
        {
            this.Writer = new WriterRepository();
            this.Reader = new ReaderRepository();
        }

        public SqlServerLogger(string connectionString)
        {
            this.Writer = new WriterRepository(connectionString);
            this.Reader = new ReaderRepository(connectionString);
        }

        private bool initialised = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Init()
        {
            if (initialised) return;
            Writer.CreateTable();
            initialised = true;
        }

        public void Log(LogEntry entry)
        {
            Init();
            Writer.PurgeOldEntries();
            Writer.AddEntry(entry);
        }

        public void Clear()
        {
            Init();
            Writer.ClearLog();
        }


        public LogResultSet Query(LogQuery criteria)
        {
            var result = new LogResultSet();
            result.TotalEntries = Reader.Count(criteria);
            if (criteria.PageSize == 0) {
                criteria.PageNumber = 1;
                criteria.PageSize = result.TotalEntries;
            }

            result.PageSize = criteria.PageSize;
            result.TotalPages = (result.TotalEntries + result.PageSize - 1) / result.PageSize;
            result.Page = criteria.PageNumber;
            if (result.Page < 1) result.Page = 1;
            if (result.Page > result.TotalPages) result.Page = result.TotalPages;
            if (result.Page >= 1)
                result.LogEntries = Reader.GetLogEntries(criteria)
                    .Skip((result.Page - 1) * result.PageSize)
                    .Take(result.PageSize).ToList();
            else
                result.LogEntries = new List<LogEntry>(0);
            return result;
        }
    }
}
