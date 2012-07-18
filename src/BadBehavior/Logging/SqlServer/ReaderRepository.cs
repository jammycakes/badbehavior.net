using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace BadBehavior.Logging.SqlServer
{
    public class ReaderRepository : RepositoryBase, IReaderRepository
    {
        public ReaderRepository() : base() { }

        public ReaderRepository(string connectionString) : base(connectionString) { }


        private static string GetWhereClause(LogQuery query)
        {
            if (!String.IsNullOrEmpty(query.Filter) && !String.IsNullOrEmpty(query.FilterValue))
                return " where [" + query.Filter + "]=@filter";
            else
                return String.Empty;
        }

        private static string GetOrderByClause(LogQuery query)
        {
            if (!String.IsNullOrEmpty(query.Sort))
                return " order by [" + query.Sort + "]";
            else
                return String.Empty;
        }

        private static SqlParameter[] GetParameters(LogQuery query)
        {
            if (!String.IsNullOrEmpty(query.Filter) && !String.IsNullOrEmpty(query.FilterValue))
                return new SqlParameter[] { new SqlParameter("@filter", query.FilterValue) };
            else
                return new SqlParameter[0];
        }

        public int Count(LogQuery query)
        {
            string sql = "select count(*) from BadBehavior_Log" + GetWhereClause(query);
            return base.ExecuteScalar<int>(sql, GetParameters(query));
        }


        public IEnumerable<LogEntry> GetLogEntries(LogQuery query)
        {
            string sql = "select * from BadBehavior_Log"
                + GetWhereClause(query) + GetOrderByClause(query);
            return base.Read(sql, ReadLogEntry, GetParameters(query));
        }

        private LogEntry ReadLogEntry(IDataRecord reader)
        {
            return new LogEntry() {
                Date = (DateTime)reader["Date"],
                HttpHeaders = reader["HttpHeaders"] as string,
                IP = IPAddress.Parse((string)reader["IP"]),
                Key = reader["Key"] as string,
                RequestEntity = reader["RequestEntity"] as string,
                RequestMethod = reader["RequestMethod"] as string,
                RequestUri = reader["RequestUri"] as string,
                ServerProtocol = reader["ServerProtocol"] as string,
                UserAgent = reader["UserAgent"] as string
            };
        }
    }
}
