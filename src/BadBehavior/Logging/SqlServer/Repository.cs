using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class Repository
    {
        protected string connectionString { get; private set; }

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
                param.Value = param.Value ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        protected SqlCommand GetCommandFromText(SqlConnection cn, string text)
        {
            var cmd = cn.CreateCommand();
            cmd.CommandText = text;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        private string GetScript(string scriptName)
        {
            var resourceName = this.GetType().Namespace + "." + scriptName;
            string script;
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream)) {
                script = reader.ReadToEnd();
            }
            return script;
        }

        protected SqlCommand GetCommandFromScript(SqlConnection cn, string scriptName)
        {
            return GetCommandFromText(cn, GetScript(scriptName));
        }
    }
}
