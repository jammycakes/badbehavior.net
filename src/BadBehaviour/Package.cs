using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BadBehaviour
{
	public class Package
	{
		/// <summary>
		///  The configuration settings to be used with this package.
		/// </summary>

		public IConfiguration Configuration { get; private set; }

		/// <summary>
		///  Gets the HTTP headers case insensitively by key.
		/// </summary>

		public IDictionary<string, string> Headers { get; private set; }

		/// <summary>
		///  Gets the <see cref="HttpRequestBase"/> instance being validated.
		/// </summary>

		public HttpRequestBase Request { get; private set; }

		/// <summary>
		///  Gets the user agent string, translated into lower case.
		/// </summary>

		public string UserAgentI { get; private set; }

		/// <summary>
		///  The originating IP address, allowing for reverse proxies if set.
		/// </summary>

		public IPAddress OriginatingIP { get; private set; }


		/* ====== Properties computed by the validators ====== */

		public bool IsBrowser { get; set; }



		/* ====== Constructor ====== */

		public Package(HttpRequestBase request, IConfiguration configuration)
		{
			this.Configuration = configuration;
			this.Request = request;
			this.Headers = new Dictionary<string, string>
				(this.Request.Headers.Count, StringComparer.InvariantCultureIgnoreCase);
			foreach (string key in request.Headers.Keys) {
				this.Headers[key] = request.Headers[key];
			}
			this.UserAgentI = this.Request.UserAgent.ToLowerInvariant();
			this.OriginatingIP = FindOriginatingIP();

			this.IsBrowser = false;
		}


		private IPAddress FindOriginatingIP()
		{
			string addr = this.Request.UserHostAddress;

			IPAddress ip;
			return IPAddress.TryParse(addr, out ip) ? ip : null;
		}
	}
}
