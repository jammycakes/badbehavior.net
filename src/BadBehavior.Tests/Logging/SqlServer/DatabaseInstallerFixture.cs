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
    public class DatabaseInstallerFixture
    {
        public static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString;

        [TestFixtureSetUp]
        public void SetupDatabase()
        {
            var installer = new DatabaseInstaller(connectionString);
            installer.InstallObjects();
        }

        [Test]
        public void CanCreateObjects()
        {
        }
    }
}
