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

		public Package(HttpRequestBase request)
		{
			this.Request = request;
			this.Headers = new Dictionary<string, string>
				(this.Request.Headers.Count, StringComparer.InvariantCultureIgnoreCase);
			foreach (string key in request.Headers.Keys) {
				this.Headers[key] = request.Headers[key];
			}
		}
	}
}
