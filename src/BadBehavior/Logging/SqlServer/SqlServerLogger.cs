using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class SqlServerLogger : Repository, ILogger
    {
        public SqlServerLogger(string connectionString)
            : base(connectionString)
        { }

        private bool initialised = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Init()
        {
            if (initialised) return;
            new DatabaseInstaller(connectionString).InstallObjects();
            initialised = true;
        }

        public void Log(LogEntry entry)
        {
            Init();
            using (var cn = Connect())
            using (var cmd = GetCommand(cn, "AddEntry",
                new SqlParameter("@IP", entry.IP.ToString()),
                new SqlParameter("@Date", entry.Date),
                new SqlParameter("@RequestMethod", entry.RequestMethod),
                new SqlParameter("@RequestUri", entry.RequestUri),
                new SqlParameter("@ServerProtocol", entry.ServerProtocol),
                new SqlParameter("@HttpHeaders", entry.HttpHeaders),
                new SqlParameter("@UserAgent", entry.UserAgent),
                new SqlParameter("@RequestEntity", entry.RequestEntity),
                new SqlParameter("@Key", entry.Key)
            )) {
                cmd.ExecuteNonQuery();
            }
        }

        public void Clear()
        {
            Init();
            using (var cn = Connect())
            using (var cmd = GetCommand(cn, "ClearLog"))
                cmd.ExecuteNonQuery();
        }


        public ILogQuery Query(int page, int pageSize, string filter)
        {
            return null;
        }
    }
}
