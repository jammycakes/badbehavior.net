using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BadBehavior.Rules;
using Moq;
using NUnit.Framework;

namespace BadBehavior.Tests.Rules
{
    [TestFixture]
    public class SearchEngineFixture : RequestFixtureBase
    {
        private void Test(string userAgent, string ipAddress)
        {
            var mock = CreateRequest(userAgent, new Dictionary<string, string>() { });
            mock.SetupGet(x => x.UserHostAddress).Returns(ipAddress);
            var package = new Package(mock.Object, new BBEngine());
            new SearchEngine().Validate(package);
        }


        [TestCase("msnbot", "207.46.0.0")]
        [TestCase("Googlebot", "66.249.64.0")]
        [TestCase("Yahoo! Slurp", "202.160.176.0")]
        public void CanValidateSearchEngines(string userAgent, string ipAddress)
        {
            Test(userAgent, ipAddress);
        }

        [TestCase("msnbot")]
        [TestCase("Googlebot")]
        [TestCase("Yahoo! Slurp")]
        [ExpectedException(typeof(BadBehaviorException))]
        public void CanInvalidateFakeSearchEngines(string userAgent)
        {
            Test(userAgent, "127.0.0.1");
        }
    }
}
