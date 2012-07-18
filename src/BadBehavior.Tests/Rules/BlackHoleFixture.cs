using System.Net;
using BadBehavior.Rules;
using NUnit.Framework;

namespace BadBehavior.Tests.Rules
{
    [TestFixture]
    public class BlackHoleFixture
    {
        [TestCase("127.0.0.1", "sbl-xbl.spamhaus.org", "1.0.0.127.sbl-xbl.spamhaus.org")]
        public void CanGetDnsblLookup(string ip, string dnsbl, string expected)
        {
            string actual = new BlackHole().GetDnsblLookup(IPAddress.Parse(ip), dnsbl);
            Assert.AreEqual(expected, actual);
        }
    }
}
