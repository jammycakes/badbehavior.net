using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BadBehavior.Logging;
using BadBehavior.Logging.SqlServer;
using NUnit.Framework;

namespace BadBehavior.Tests.Logging.SqlServer
{
    [TestFixture]
    public class ReaderRepositoryFixture
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var logger = new SqlServerLogger();
            logger.Init();
            logger.Clear();
        }

        [SetUp]
        public void BeforeTest()
        {
            var logger = new SqlServerLogger();
            logger.Log(new LogEntry() {
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
            var logger = new SqlServerLogger();
            logger.Clear();
        }

        [Test]
        public void CanCount()
        {
            var query = new LogQuery();
            var repo = new ReaderRepository();
            var count = repo.Count(query);
            Assert.Greater(count, 0);
        }

        [TestCase("IP", "::1", 1)]
        [TestCase("IP", "::2", 0)]
        [TestCase("Key", "00000000", 1)]
        public void CanCountFiltered(string filter, string filterValue, int expected)
        {
            var query = new LogQuery() {
                Filter = filter,
                FilterValue = filterValue
            };

            var repo = new ReaderRepository();
            Assert.AreEqual(expected, repo.Count(query));
        }
    }
}
