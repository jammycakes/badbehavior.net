using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace BadBehavior.Tests
{
    [TestFixture]
    public class FunctionsFixture
    {
        [TestCase("127.0.0.1", "127.0.0.0/8", true)]
        [TestCase("127.1.0.1", "127.0.0.0/8", true)]
        [TestCase("127.255.0.1", "127.0.0.0/8", true)]
        [TestCase("128.0.0.0", "127.0.0.0/8", false)]
        [TestCase("126.255.255.255", "127.0.0.0/8", false)]
        [TestCase("254.0.0.1", "254.0.0.0/8", true)]
        [TestCase("254.1.0.1", "254.0.0.0/8", true)]
        [TestCase("254.255.0.1", "254.0.0.0/8", true)]
        [TestCase("255.0.0.0", "254.0.0.0/8", false)]
        [TestCase("253.255.255.255", "254.0.0.0/8", false)]
        [TestCase("127.0.0.1", "127.0.0.0", false)]
        [TestCase("127.0.0.1", "127.0.0.1", true)]
        [TestCase("254.0.0.1", "254.0.0.0", false)]
        [TestCase("254.0.0.1", "254.0.0.1", true)]
        public void CanMatchCidr(string addr, string cidr, bool expected)
        {
            var ipAddress = IPAddress.Parse(addr);
            Assert.AreEqual(expected, ipAddress.MatchCidr(cidr));
        }
    }
}
