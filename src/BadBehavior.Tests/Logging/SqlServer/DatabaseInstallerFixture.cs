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
    public class DatabaseInstallerFixture : RepositoryBase
    {
        [TestFixtureSetUp]
        public void SetupDatabase()
        {
            var installer = new SqlServerLogger();
            installer.Init();
        }

        [Test]
        public void CanCreateObjects()
        {
            string s = "select name from dbo.sysobjects where xtype='P'";
            CollectionAssert.Contains(this.Read(s, x => x.GetString(0)), "BadBehavior_AddEntry");
        }
    }
}
