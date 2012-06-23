﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Moq;
using NUnit.Framework;

namespace BadBehaviour.Tests
{
	[TestFixture]
	public class PackageFixture
	{
		private HttpRequestBase CreateRequest(string ipAddress, string forwardedFor)
		{
			var request = new Mock<HttpRequestBase>();
			var headers = new NameValueCollection();
			if (forwardedFor != null) {
				headers.Add(Configuration.DefaultReverseProxyHeader, forwardedFor);
			}
			request.SetupGet(x => x.UserHostAddress).Returns(ipAddress);
			request.SetupGet(x => x.Headers).Returns(headers);
			return request.Object;
		}

		[TestCase("1.2.3.4", null, "1.2.3.4")]
		[TestCase("1.2.3.4", "12.34.56.78", "12.34.56.78")]
		public void CanFindOriginatingIP(string ipAddress, string forwardedFor, string expected,
			params string[] knownProxies)
		{
			var request = CreateRequest(ipAddress, forwardedFor);
			var configuration = new Mock<IConfiguration>(MockBehavior.Loose);
			configuration.SetupGet(x => x.ReverseProxy).Returns(forwardedFor != null);
			configuration.SetupGet(x => x.ReverseProxyHeader).Returns(Configuration.DefaultReverseProxyHeader);
			configuration.SetupGet(x => x.ReverseProxyAddresses).Returns(new List<string>(knownProxies));
			var package = new Package(request, configuration.Object);
			Assert.AreEqual(expected, package.OriginatingIP.ToString());
		}
	}
}
