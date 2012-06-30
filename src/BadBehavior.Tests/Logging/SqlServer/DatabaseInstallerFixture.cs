using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BadBehavior.Logging.SqlServer;
using NUnit.Framework;

namespace BadBehavior.Tests.Logging.SqlServer
{
    [TestFixture]
    public class DatabaseInstallerFixture : Repository
    {
        public static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString;

        public DatabaseInstallerFixture()
            : base(connectionString)
        { }

        [TestFixtureSetUp]
        public void SetupDatabase()
        {
            var installer = new DatabaseInstaller(connectionString);
            installer.InstallObjects();
        }

        [Test]
        public void CanCreateObjects()
        {
            string s = "select name from dbo.sysobjects where xtype='P'";
            using (var cn = Connect())
            using (var cmd = this.GetCommandFromText(cn, s))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    if (reader.GetString(0) == "BadBehavior_AddEntry") return;
            Assert.Fail("Stored procedure BadBehavior_AddEntry was not found in the database.");

        }
    }
}
