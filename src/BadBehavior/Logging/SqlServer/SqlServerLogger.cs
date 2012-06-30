using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class SqlServerLogger : Repository, ILogWriter
    {
        public SqlServerLogger(string connectionString)
            : base(connectionString)
        { }

        public void Log(LogEntry entry)
        {
            using (var cn = Connect())
            using (var cmd = GetCommand(cn, "AddEntry",
                new SqlParameter("@IP", entry.IP),
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
    }
}
