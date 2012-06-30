using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BadBehavior
{
    public class Configuration : IConfiguration
    {
        public const string DefaultReverseProxyHeader = "X-Forwarded-For";

        public static Configuration Instance { get; set; }

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

        public bool OffsiteForms { get; private set; }

        public bool Strict { get; private set; }

        public string SupportEmail { get; private set; }

        public bool ReverseProxy { get; private set; }

        public string ReverseProxyHeader { get; private set; }

        public IList<string> ReverseProxyAddresses { get; private set; }

        public Configuration()
        {
            this.OffsiteForms = GetBool("OffsiteForms", false);
            this.Strict = GetBool("Strict", false);
            this.SupportEmail = GetString("SupportEmail", null);
            this.ReverseProxy = GetBool("ReverseProxy", false);
            this.ReverseProxyHeader = GetString("ReverseProxyHeader", DefaultReverseProxyHeader);
            var reverseProxyAddresses = GetString("ReverseProxyAddresses", null);
            if (reverseProxyAddresses == null)
                this.ReverseProxyAddresses = new List<string>().AsReadOnly();
            else
                this.ReverseProxyAddresses = reverseProxyAddresses.Split(',', ';').ToList().AsReadOnly();
        }

        static Configuration()
        {
            Instance = new Configuration();
        }
    }
}
