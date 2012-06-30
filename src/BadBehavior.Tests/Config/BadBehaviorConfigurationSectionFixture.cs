using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BadBehavior.Configuration;
using NUnit.Framework;

namespace BadBehavior.Tests.Config
{
    [TestFixture]
    public class BadBehaviorConfigurationSectionFixture
    {
        private IConfiguration config;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this.config = ConfigurationManager.GetSection("badBehavior")
                as BadBehaviorConfigurationSection;
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
            CollectionAssert.AreEqual(new string[] { "test.one", "test.two" }, addresses);
        }
    }
}
