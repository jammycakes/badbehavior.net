using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBehavior.Admin;
using BadBehavior.Logging;
using NUnit.Framework;

namespace BadBehavior.Tests.Admin
{
    [TestFixture]
    public class PagerFixture
    {
        [TestCase(1, 10, 1, 2, 3, 0, 10)]
        [TestCase(2, 10, 1, 2, 3, 4, 0, 10)]
        [TestCase(3, 10, 1, 2, 3, 4, 5, 0, 10)]
        [TestCase(4, 10, 1, 2, 3, 4, 5, 6, 0, 10)]
        [TestCase(5, 10, 1, 0, 3, 4, 5, 6, 7, 0, 10)]
        [TestCase(6, 10, 1, 0, 4, 5, 6, 7, 8, 0, 10)]
        [TestCase(7, 10, 1, 0, 5, 6, 7, 8, 9, 10)]
        [TestCase(8, 10, 1, 0, 6, 7, 8, 9, 10)]
        [TestCase(9, 10, 1, 0, 7, 8, 9, 10)]
        [TestCase(10, 10, 1, 0, 8, 9, 10)]

        [TestCase(4, 8, 1, 2, 3, 4, 5, 6, 0, 8)]
        [TestCase(4, 7, 1, 2, 3, 4, 5, 6, 7)]
        [TestCase(3, 6, 1, 2, 3, 4, 5, 6)]
        [TestCase(1, 4, 1, 2, 3, 4)]
        [TestCase(1, 2, 1, 2)]
        [TestCase(2, 2, 1, 2)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 3, 1, 2, 3)]

        public void CanGetPageNumbers(int pageNumber, int totalPages, params int[] expected)
        {
            var query = new LogQuery() {
                PageNumber = pageNumber
            };
            var results = new LogResultSet() {
                Page = pageNumber,
                TotalPages = totalPages
            };
            var pageNumbers = new Pager(query, results).GetPageNumbers();
            CollectionAssert.AreEqual(expected, pageNumbers);
        }
    }
}
