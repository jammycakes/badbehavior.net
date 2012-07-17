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
            return null;
        }
    }
}
