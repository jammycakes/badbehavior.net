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
        public Repository Repository { get; set; }

        public SqlServerLogger()
        {
            this.Repository = new Repository();
        }

        public SqlServerLogger(string connectionString)
        {
            this.Repository = new Repository(connectionString);
        }

        private bool initialised = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Init()
        {
            if (initialised) return;
            Repository.CreateTable();
            initialised = true;
        }

        public void Log(LogEntry entry)
        {
            Init();
            Repository.PurgeOldEntries();
            Repository.AddEntry(entry);
        }

        public void Clear()
        {
            Init();
            Repository.ClearLog();
        }


        public ILogQuery Query(int page, int pageSize, string filter)
        {
            return null;
        }
    }
}
