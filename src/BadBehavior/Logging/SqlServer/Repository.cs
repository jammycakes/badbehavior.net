using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class Repository : RepositoryBase
    {
        public Repository(string connectionString)
            : base(connectionString)
        {
        }

        public Repository() : base()
        {
        }

        public void CreateTable()
        {
            string sql = GetQueryFromResource("CreateTable");
            base.ExecuteNonQuery(sql);
        }

        public void PurgeOldEntries()
        {
            base.ExecuteNonQuery
                ("delete from BadBehavior_Log where [Date] < dateadd(dd, -7, getdate())");
        }

        public void ClearLog()
        {
            base.ExecuteNonQuery("delete from BadBehavior_Log");
        }

        public void AddEntry(LogEntry entry)
        {
            string sql = GetQueryFromResource("AddEntry");
            base.ExecuteNonQuery(sql,
                new SqlParameter("@IP", entry.IP.ToString()) { Size = 40 },
                new SqlParameter("@Date", entry.Date),
                new SqlParameter("@RequestMethod", entry.RequestMethod) { Size = 16 },
                new SqlParameter("@RequestUri", entry.RequestUri),
                new SqlParameter("@ServerProtocol", entry.ServerProtocol) { Size = 16 },
                new SqlParameter("@HttpHeaders", entry.HttpHeaders),
                new SqlParameter("@UserAgent", entry.UserAgent),
                new SqlParameter("@RequestEntity", entry.RequestEntity),
                new SqlParameter("@Key", entry.Key) { Size = 8 }
            );
        }
    }
}
