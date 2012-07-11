using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class SqlServerLogReader : SqlObject, ILogReader
    {
        public SqlServerLogReader(string connectionString)
            : base(connectionString)
        { }

        public IEnumerable<LogEntry> Read(int skip, int take)
        {
            // TODO: this could be implemented more efficiently in SQL.
            return ReadAll().Skip(skip).Take(take);
        }

        public IEnumerable<LogEntry> ReadAll()
        {
            return Read(null, null);
        }

        public IEnumerable<LogEntry> Read(DateTime start, DateTime end)
        {
            return Read((DateTime?)start, (DateTime?)end);
        }

        private IEnumerable<LogEntry> Read(DateTime? start, DateTime? end)
        {
            var result = new List<LogEntry>();
            using (var cn = this.Connect())
            using (var cmd = this.GetCommand(cn, "Read",
                new SqlParameter("@Start", start),
                new SqlParameter("@End", end)
            ))
            using (var reader = this.ExecuteReader(cmd)) {
                while (reader.Read()) {
                    result.Add(this.ReadEntry(reader));
                }
            }
            return result;
        }

        private LogEntry ReadEntry(SqlDataReader reader)
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

        public long Count()
        {
            using (var cn = this.Connect())
            using (var cmd = this.GetCommand(cn, "Count",
                new SqlParameter("@Start", null),
                new SqlParameter("@End", null)
            )) {
                return this.ExecuteScalar<int>(cmd);
            }
        }

        public long Count(DateTime start, DateTime end)
        {
            using (var cn = this.Connect())
            using (var cmd = this.GetCommand(cn, "Count",
                new SqlParameter("@Start", start),
                new SqlParameter("@End", end)
            )) {
                return this.ExecuteScalar<int>(cmd);
            }
        }
    }
}
