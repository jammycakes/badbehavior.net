using System.Configuration;
using System.Linq;
using BadBehavior.Configuration;
using NUnit.Framework;

namespace BadBehavior.Tests.Config
{
    [TestFixture]
    public class BadBehaviorConfigurationSectionFixture
    {
        private BadBehaviorConfigurationSection config;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this.config = BadBehaviorConfigurationSection.Get();
        }

        [Test]
        public void CanLoadConfiguration()
        {
            // A couple of quick sanity checks to ensure the config section is loading.

            Assert.IsTrue(config.OffsiteForms);
            Assert.AreEqual("no-reply@jamesmckay.net", config.SupportEmail);
            Assert.AreEqual(BadBehaviorConfigurationSection.DefaultReverseProxyHeader,
                config.ReverseProxyHeader);
        }

        [Test]
        public void CanLoadReverseProxyAddresses()
        {
            var addresses = config.ReverseProxyAddresses;
            CollectionAssert.AreEqual(
                new string[] { "test.one", "test.two" },
                addresses.Cast<ValueElement>().Select(x => x.Value)
            );
        }

        [Test]
        public void CanLoadWhiteListIPRanges()
        {
            var ipRanges = config.WhiteList.IPRanges;
            CollectionAssert.AreEqual(
                new string[] { "10.0.0.0/8" },
                ipRanges.Cast<ValueElement>().Select(x => x.Value)
            );
        }

        [Test]
        public void CanLoadWhiteListUrls()
        {
            var urls = config.WhiteList.Urls;
            CollectionAssert.IsEmpty(urls);
        }
    }
}
