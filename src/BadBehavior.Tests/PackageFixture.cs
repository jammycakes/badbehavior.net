using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using BadBehavior.Configuration;
using Moq;
using NUnit.Framework;

namespace BadBehavior.Tests
{
    [TestFixture]
    public class PackageFixture
    {
        private HttpRequestBase CreateRequest(string ipAddress, string forwardedFor)
        {
            var request = new Mock<HttpRequestBase>();
            var headers = new NameValueCollection();
            if (forwardedFor != null) {
                headers.Add(BadBehaviorConfigurationSection.DefaultReverseProxyHeader, forwardedFor);
            }
            request.SetupGet(x => x.UserHostAddress).Returns(ipAddress);
            request.SetupGet(x => x.Headers).Returns(headers);
            return request.Object;
        }

        [TestCase("1.2.3.4", null, "1.2.3.4")]
        [TestCase("1.2.3.4", "12.34.56.78", "12.34.56.78")]
        [TestCase("1.2.3.4", "5.6.7.8 12.34.56.78", "5.6.7.8")]
        [TestCase("1.2.3.4", "5.6.7.8 12.34.56.78", "12.34.56.78", "5.0.0.0/8")]
        [TestCase("1.2.3.4", "192.168.3.4,5.6.7.8,12.34.56.78", "12.34.56.78", "5.0.0.0/8")]
        public void CanFindOriginatingIP(string ipAddress, string forwardedFor, string expected,
            params string[] knownProxies)
        {
            var request = CreateRequest(ipAddress, forwardedFor);
            var settings = new Mock<ISettings>(MockBehavior.Loose);
            settings.SetupGet(x => x.ReverseProxy).Returns(forwardedFor != null);
            settings.SetupGet(x => x.ReverseProxyHeader)
                .Returns(BadBehaviorConfigurationSection.DefaultReverseProxyHeader);
            settings.SetupGet(x => x.ReverseProxyAddresses).Returns(new List<string>(knownProxies));
            var package = new Package(request, new BBEngine(settings.Object));
            Assert.AreEqual(expected, package.OriginatingIP.ToString());
        }

        [Test]
        public void CanSetHeadersMixed()
        {
            var headers = new NameValueCollection();
            headers.Add("Test-Hyphen", "alpha");
            headers.Add("test-hyphen", "bravo");

            var mock = new Mock<HttpRequestBase>();
            mock.SetupGet(x => x.Headers).Returns(headers);
            var package = new Package(mock.Object, new BBEngine());

            CollectionAssert.AreEqual(new string[] { "alpha", "bravo" }, package.HeadersMixed.GetValues("Test-Hyphen"));
        }
    }
}
