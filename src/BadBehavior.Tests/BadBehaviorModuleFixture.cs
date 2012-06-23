using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BadBehavior.Validators;
using Moq;
using NUnit.Framework;

namespace BadBehavior.Tests
{
    [TestFixture]
    public class BadBehaviorModuleFixture : RequestFixtureBase
    {
        [TestCase("127.0.0.1", "deadbeef", "7f00-0001-dead-beef")]
        [TestCase("1.0.0.1", "deadbeef", "0100-0001-dead-beef")]
        public void CanBuildSupportKey(string ip, string code, string expected)
        {
            var ipAddress = IPAddress.Parse(ip);
            var actual = BadBehaviorModule.BuildSupportKey(ipAddress, code);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null)]
        [TestCase("test@jamesmckay.net")]
        public void CanBuildResponse(string email)
        {
            var mock = CreateRequest(
                "Mozilla/4.0 (compatible; MSIE 6.0b; Windows NT 5.1)",
                new Dictionary<string, string> { }
            );
            mock.SetupGet(x => x.UserHostAddress).Returns("127.0.0.1");
            var mockConfig = new Mock<IConfiguration>(MockBehavior.Loose);
            mockConfig.SetupGet(x => x.ReverseProxy).Returns(false);
            mockConfig.SetupGet(x => x.SupportEmail).Returns(email);

            var package = new Package(mock.Object, mockConfig.Object);
            try {
                new Browser().Validate(package);
            }
            catch (BadBehaviorException ex) {
                var content = BadBehaviorModule.GetResponseContent(ex);
                Console.WriteLine(content);
                Assert.IsFalse(content.Contains("{{email?}}"), "Email only block was not removed.");
                return;
            }
            Assert.Fail("The request validated incorrectly.");
        }
    }
}
