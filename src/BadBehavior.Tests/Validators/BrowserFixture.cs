using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using BadBehavior.Validators;
using Moq;
using NUnit.Framework;

namespace BadBehavior.Tests.Validators
{
    [TestFixture]
    public class BrowserFixture : RequestFixtureBase
    {
        [Test]
        [ExpectedException(typeof(BadBehaviorException))]
        public void MSIEAcceptRequiredAndMissing()
        {
            var request = CreateRequest(
                "Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
                new Dictionary<string, string> { }
            );
            var package = new Package(request.Object, Configuration.Instance);
            new Browser().Validate(package);
        }

        [Test]
        public void MSIEAcceptRequiredAndPresent()
        {
            var request = CreateRequest(
                "Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
                new Dictionary<string, string> {
                    { "Accept", "text/html" }
                }
            );
            var package = new Package(request.Object, Configuration.Instance);
            new Browser().Validate(package);
        }

        [Test]
        [ExpectedException(typeof(BadBehaviorException))]
        public void MSIEDoesNotSendConnectionTE()
        {
            var request = CreateRequest(
                "Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
                new Dictionary<string, string> {
                    { "Accept", "text/html" },
                    { "Connection", "TE" }
                }
            );
            var package = new Package(request.Object, Configuration.Instance);
            new Browser().Validate(package);
        }

        [Test]
        public void AkamaiSendsConnectionTE()
        {
            var request = CreateRequest(
                "Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
                new Dictionary<string, string> {
                    { "Accept", "text/html" },
                    { "Connection", "TE" },
                    { "Akamai-Origin-Hop", "1" },
                }
            );
            var package = new Package(request.Object, Configuration.Instance);
            new Browser().Validate(package);
        }

        [Test]
        public void IEMobileSendsConnectionTE()
        {
            var request = CreateRequest(
                "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)",
                new Dictionary<string, string> {
                    { "Accept", "text/html" },
                    { "Connection", "TE" },
                }
            );
            var package = new Package(request.Object, Configuration.Instance);
            new Browser().Validate(package);
        }
    }
}
