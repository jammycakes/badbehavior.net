using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace BadBehavior.Tests
{
    [TestFixture]
    public class BadBehaviorModuleFixture
    {
        [TestCase("127.0.0.1", "deadbeef", "7f00-0001-dead-beef")]
        [TestCase("1.0.0.1", "deadbeef", "0100-0001-dead-beef")]
        public void CanBuildSupportKey(string ip, string code, string expected)
        {
            var ipAddress = IPAddress.Parse(ip);
            var actual = BadBehaviorModule.BuildSupportKey(ipAddress, code);
            Assert.AreEqual(expected, actual);
        }
    }
}
