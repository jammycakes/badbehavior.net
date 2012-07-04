using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BadBehavior.Configuration
{
    public class BadBehaviorConfigurationSection : ConfigurationSection, ISettings
    {
        public const string DefaultReverseProxyHeader = "X-Forwarded-For";

        [ConfigurationProperty("offsiteForms", DefaultValue = false, IsRequired = false)]
        public bool OffsiteForms
        {
            get { return (bool)this["offsiteForms"]; }
            set { this["offsiteForms"] = value; }
        }

        [ConfigurationProperty("strict", DefaultValue = false, IsRequired = false)]
        public bool Strict
        {
            get { return (bool)this["strict"]; }
            set { this["strict"] = value; }
        }

        [ConfigurationProperty("supportEmail", IsRequired = false)]
        public string SupportEmail
        {
            get { return (string)this["supportEmail"]; }
            set { this["supportEmail"] = value; }
        }

        [ConfigurationProperty("reverseProxy", DefaultValue = false, IsRequired = false)]
        public bool ReverseProxy
        {
            get { return (bool)this["reverseProxy"]; }
            set { this["reverseProxy"] = value; }
        }

        [ConfigurationProperty("reverseProxyAddresses", IsDefaultCollection = false)]
        public ValueCollection ReverseProxyAddresses
        {
            get { return (ValueCollection)this["reverseProxyAddresses"]; }
            set { this["reverseProxyAddresses"] = value; }
        }

        IList<string> ISettings.ReverseProxyAddresses
        {
            get {
                return
                    this.ReverseProxyAddresses.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        [ConfigurationProperty("reverseProxyHeader", DefaultValue = DefaultReverseProxyHeader, IsRequired = false)]
        public string ReverseProxyHeader
        {
            get { return (string)this["reverseProxyHeader"]; }
            set { this["reverseProxyHeader"] = value; }
        }

        [ConfigurationProperty("whitelist")]
        public WhiteListElement WhiteList
        {
            get { return (WhiteListElement)this["whitelist"]; }
            set { this["whitelist"] = value; }
        }

        [ConfigurationProperty("httpbl")]
        public HttpblElement Httpbl
        {
            get { return (HttpblElement)this["httpbl"]; }
            set { this["httpbl"] = value; }
        }

        [ConfigurationProperty("debug", IsRequired = false, DefaultValue = false)]
        public bool Debug
        {
            get { return (bool)this["debug"]; }
            set { this["debug"] = value; }
        }

        IList<string> ISettings.WhitelistIPRanges
        {
            get {
                return
                    this.WhiteList.IPRanges.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        IList<string> ISettings.WhitelistUserAgents
        {
            get
            {
                return
                    this.WhiteList.UserAgents.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        IList<string> ISettings.WhitelistUrls
        {
            get
            {
                return
                    this.WhiteList.Urls.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        bool ISettings.Httpbl
        {
            get
            {
                return this.Httpbl != null && !String.IsNullOrEmpty(this.Httpbl.Key);
            }
        }

        string ISettings.HttpblKey
        {
            get
            {
                return Httpbl.Key;
            }
        }


        int ISettings.HttpblThreatLevel
        {
            get
            {
                return Httpbl.ThreatLevel;
            }
        }

        int ISettings.HttpblMaxAge
        {
            get
            {
                return Httpbl.MaxAge;
            }
        }
    }
}
