using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BadBehaviour
{
	public class Package
	{
		public IDictionary<string, string> Headers { get; private set; }

		public HttpRequestBase Request { get; private set; }

		/// <summary>
		///  Gets the user agent string, translated into lower case.
		/// </summary>

		public string UserAgentI { get; private set; }


		/* ====== Properties computed by the validators ====== */

		public bool IsBrowser { get; set; }


		/* ====== Constructor ====== */

		public Package(HttpRequestBase request)
		{
			this.Request = request;
			this.Headers = new Dictionary<string, string>
				(this.Request.Headers.Count, StringComparer.InvariantCultureIgnoreCase);
			foreach (string key in request.Headers.Keys) {
				this.Headers[key] = request.Headers[key];
			}
			this.UserAgentI = this.Request.UserAgent.ToLowerInvariant();

			this.IsBrowser = false;
		}
	}
}
