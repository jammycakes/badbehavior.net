using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class Repository
    {
        private string connectionString;

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected SqlConnection Connect()
        {
            var cn = new SqlConnection(this.connectionString);
            cn.Open();
            return cn;
        }

        protected SqlCommand GetCommand
            (SqlConnection cn, string commandName, params SqlParameter[] parameters)
        {
            var cmd = cn.CreateCommand();
            cmd.CommandText = "BadBehavior_" + commandName;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters) {
                cmd.Parameters.Add(param);
            }
            return cmd;
        }
    }
}
