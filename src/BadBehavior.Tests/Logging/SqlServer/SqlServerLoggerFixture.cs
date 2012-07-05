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
    public class SqlServerLoggerFixture
    {
        private ILogReader reader;
        private ILogWriter writer;

        [TestFixtureSetUp]
        public void Init()
        {
            string cs = ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString;
            reader = new SqlServerLogReader(cs);
            writer = new SqlServerLogger(cs);
        }

        [Test]
        public void CanLogError()
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

        [Test]
        public void CanCount()
        {
            // Force an entry into the log.
            CanLogError();
            Assert.Greater(reader.Count(), 0);
        }
    }
}
