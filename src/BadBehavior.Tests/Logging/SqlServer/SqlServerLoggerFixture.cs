using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBehavior.Logging;
using BadBehavior.Logging.SqlServer;
using Moq;
using NUnit.Framework;

namespace BadBehavior.Tests.Logging.SqlServer
{
    [TestFixture]
    public class SqlServerLoggerFixture
    {
        // No records should always return page 0, page count 0.
        [TestCase(0, 10, 1, 0, 0)]
        [TestCase(0, 10, 0, 0, 0)]
        // One page of records should always return page one
        [TestCase(1, 10, 1, 1, 1)]
        [TestCase(1, 10, 0, 1, 1)]
        [TestCase(1, 10, 5, 1, 1)]
        [TestCase(10, 10, 5, 1, 1)]
        [TestCase(10, 10, 0, 1, 1)]
        // More than one page of records should always return a page within range
        [TestCase(11, 10, 0, 1, 2)]
        [TestCase(11, 10, 1, 1, 2)]
        [TestCase(11, 10, 2, 2, 2)]
        [TestCase(11, 10, 3, 2, 2)]
        [TestCase(20, 10, 0, 1, 2)]
        [TestCase(20, 10, 1, 1, 2)]
        [TestCase(20, 10, 2, 2, 2)]
        [TestCase(20, 10, 3, 2, 2)]
        // If no page size is specified, return everything.
        [TestCase(20, 0, 0, 1, 1)]
        [TestCase(20, 0, 1, 1, 1)]
        [TestCase(20, 0, 2, 1, 1)]
        public void CanCreatePaging
            (int numRecords, int pageSize, int requestedPage, int expectedPage, int expectedPageCount)
        {
            var query = new LogQuery() {
                PageSize = pageSize,
                PageNumber = requestedPage
            };
            var reader = new Mock<IReaderRepository>();
            reader.Setup(x => x.Count(It.IsAny<LogQuery>())).Returns(numRecords);
            var writer = new Mock<IWriterRepository>();
            writer.SetupAllProperties();
            var logger = new SqlServerLogger() { Reader = reader.Object, Writer = writer.Object };
            var result = logger.Query(query);
            Assert.AreEqual(expectedPage, result.Page, "Wrong page number");
            Assert.AreEqual(expectedPageCount, result.TotalPages, "Wrong number of pages");
        }
    }
}
