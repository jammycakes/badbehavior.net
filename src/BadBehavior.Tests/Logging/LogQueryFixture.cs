using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using BadBehavior.Logging;
using NUnit.Framework;

namespace BadBehavior.Tests.Logging
{
    [TestFixture]
    public class LogQueryFixture
    {
        [Test]
        public void CanSetFilter()
        {
            var query = new NameValueCollection();
            query.Add("filter", "IP");
            query.Add("filtervalue", "::1");
            var q = new LogQuery(query);
            Assert.AreEqual("IP", q.Filter);
            Assert.AreEqual("::1", q.FilterValue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotSetInvalidFilter()
        {
            var query = new NameValueCollection();
            query.Add("filter", "Robert'); drop table students; --"); // http://xkcd.com/327
            query.Add("filtervalue", "::1");
            new LogQuery(query);
        }

        [Test]
        public void CanSetSortOrder()
        {
            var query = new NameValueCollection();
            query.Add("sort", "IP");
            var q = new LogQuery(query);
            Assert.AreEqual("IP", q.Sort);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CanNotSetInvalidSort()
        {
            var query = new NameValueCollection();
            query.Add("sort", "Robert'); drop table students; --");
            new LogQuery(query);
        }

        [TestCase("asc", true)]
        [TestCase("desc", false)]
        [TestCase(null, true)]
        public void CanSetPageNumberAndOrdering(string order, bool expectedOrder)
        {
            var query = new NameValueCollection();
            query.Add("page", "5");
            query.Add("order", order);
            var q = new LogQuery(query);
            Assert.AreEqual(5, q.PageNumber);
            Assert.AreEqual(expectedOrder, q.Ascending);
        }
    }
}
