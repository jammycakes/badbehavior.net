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
        private ILogReader reader;
        private ILogger writer;

        [TestFixtureSetUp]
        public void Init()
        {
            string cs = ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString;
            reader = new SqlServerLogReader(cs);
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
        public void CanCount()
        {
            Assert.Greater(reader.Count(), 0);
        }

        [Test]
        public void CanReadAll()
        {
            foreach (var entry in reader.ReadAll()) {
                Assert.IsNotNull(entry);
            }
        }

        [Test]
        public void CanReadByDate()
        {
            var entries = reader.Read(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
            CollectionAssert.IsNotEmpty(entries);

            foreach (var entry in entries) {
                Assert.IsNotNull(entry);
            }
        }

        [Test]
        public void CanClearLog()
        {
            writer.Clear();
            var count = reader.Count();
            Assert.AreEqual(0, count);
        }
    }
}
