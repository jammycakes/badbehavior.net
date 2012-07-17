using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class SqlServerLogger : ILogger
    {
        public IWriterRepository Writer { get; set; }

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
            result.PageSize = criteria.PageSize;
            result.TotalEntries = Reader.Count(criteria);
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
