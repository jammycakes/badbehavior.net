using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using BadBehavior.Logging;
using BadBehavior.Logging.SqlServer;
using NUnit.Framework;

namespace BadBehavior.Tests.Logging.SqlServer
{
    [TestFixture]
    public class SqlServerLoggerFixtureWithData
    {
        private ILogger writer;

        [TestFixtureSetUp]
        public void Init()
        {
            string cs = ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString;
            writer = new SqlServerLogger(cs);
            // Force an entry into the log.
        }

        [SetUp]
        public void BeforeTest()
        {
            writer.Log(new LogEntry() {
                Date = DateTime.Now,
                HttpHeaders = null,
                IP = IPAddress.Parse("::1"),
                Key = "00000000",
                RequestEntity = null,
                RequestMethod = "POST",
                RequestUri = "http://example.com/",
                ServerProtocol = "HTTP/1.1",
                UserAgent = "Mozilla/1.0"
            });
        }

        [TearDown]
        public void AfterTest()
        {
            writer.Clear();
        }


        [Test]
        public void CanClearLog()
        {
            var reader = new ReaderRepository();
            var query = new LogQuery();
            writer.Clear();
            var count = reader.Count(query);
            Assert.AreEqual(0, count);
        }
    }
}
