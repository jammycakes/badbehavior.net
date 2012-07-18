using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class RepositoryBase
    {
        public string ConnectionString { get; private set; }

        public RepositoryBase(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public RepositoryBase()
            : this(ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString)
        {
        }


        protected T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            using (var cn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, cn)) {
                cn.Open();
                foreach (var p in parameters) {
                    if (p.Value == null) p.Value = DBNull.Value;
                }
                cmd.Parameters.AddRange(parameters);
                var obj = cmd.ExecuteScalar();
                return obj is T ? (T)obj : default(T);
            }
        }

        protected int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var cn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, cn)) {
                cn.Open();
                foreach (var p in parameters) {
                    if (p.Value == null) p.Value = DBNull.Value;
                }
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }


        protected IEnumerable<T> Read<T>(string query, Func<IDataRecord, T> map,
            params SqlParameter[] parameters)
        {
            using (var cn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(query, cn)) {
                cn.Open();
                foreach (var p in parameters) {
                    if (p.Value == null) p.Value = DBNull.Value;
                }
                cmd.Parameters.AddRange(parameters);
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read())
                        yield return map(reader);
                }
            }
        }


        protected string GetQueryFromResource(string name)
        {
            string resourceName = this.GetType().Namespace + "." + name + ".sql";
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
