using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging.SqlServer
{
    public class DatabaseInstaller : Repository
    {
        public DatabaseInstaller(string connectionString)
            : base(connectionString)
        { }

        private IEnumerable<string> GetSprocNames()
        {
            Type t = this.GetType();
            string prefix = t.Namespace + ".Sproc.";
            return
                from name in t.Assembly.GetManifestResourceNames()
                where name.StartsWith(prefix) && name.EndsWith(".sql")
                select name.Substring(prefix.Length, name.Length - prefix.Length - 4);
        }

        private void ExecuteScript(string scriptName)
        {
            using (var cn = this.Connect())
            using (var cmd = this.GetCommandFromScript(cn, scriptName))
                cmd.ExecuteNonQuery();
        }

        private void InstallStoredProcedure(string name)
        {
            string sprocName = "BadBehavior_" + name;
            string scriptName = "Sproc." + name + ".sql";

            string dropSql = String.Format(
                "if exists (select * from sysobjects where name='{0}' and xtype='P') drop procedure {0}",
                sprocName
            );
            using (var cn = this.Connect()) {
                using (var cmd = this.GetCommandFromText(cn, dropSql)) cmd.ExecuteNonQuery();
                using (var cmd = this.GetCommandFromScript(cn, scriptName)) cmd.ExecuteNonQuery();
            }
        }

        public void InstallObjects()
        {
            ExecuteScript("CreateTable.sql");
            foreach (var name in GetSprocNames()) {
                InstallStoredProcedure(name);
            }
        }
    }
}
