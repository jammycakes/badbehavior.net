using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Moq;

namespace BadBehavior.Tests
{
    public class RequestFixtureBase
    {
        protected Mock<HttpRequestBase> CreateRequest(string userAgent, IDictionary<string, string> headers)
        {
            var mock = new Mock<HttpRequestBase>();
            mock.SetupGet(x => x.UserAgent).Returns(userAgent);
            var h = new NameValueCollection();
            foreach (string key in headers.Keys)
                h.Add(key, headers[key]);
            mock.SetupGet(x => x.Headers).Returns(h);
            return mock;
        }

    }
}
