using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BadBehaviour
{
	public class Configuration
	{
		private static string GetString(string name, string defaultValue)
		{
			return ConfigurationManager.AppSettings["BadBehaviour." + name]
				?? ConfigurationManager.AppSettings["BadBehavior." + name]
				?? defaultValue;
		}

		private static bool GetBool(string name, bool defaultValue)
		{
			string value = GetString(name, null);
			if (value == null) return defaultValue;
			bool result;
			return Boolean.TryParse(value, out result) ? result : defaultValue;
		}

		private static int GetInt(string name, int defaultValue)
		{
			string value = GetString(name, null);
			if (value == null) return defaultValue;
			int result;
			return Int32.TryParse(value, out result) ? result : defaultValue;
		}


		public bool ReverseProxy { get; private set; }

		public string ReverseProxyHeader { get; private set; }

		public IList<string> ReverseProxyAddresses { get; private set; }

		public Configuration()
		{
			this.ReverseProxy = GetBool("ReverseProxy", false);
			this.ReverseProxyHeader = GetString("ReverseProxyHeader", "X-Forwarded-For");
			var reverseProxyAddresses = GetString("ReverseProxyAddresses", null);
			if (reverseProxyAddresses == null)
				this.ReverseProxyAddresses = new List<string>().AsReadOnly();
			else
				this.ReverseProxyAddresses = reverseProxyAddresses.Split(',', ';').ToList().AsReadOnly();
		}
	}
}
