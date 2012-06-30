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
        [Test]
        public void CanLogError()
        {
            var writer = new SqlServerLogger
                (ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString);
            writer.Log(new LogEntry() {
                Date = DateTime.Parse("2012-07-01 00:00:00"),
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
    }
}
