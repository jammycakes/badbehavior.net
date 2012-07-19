using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BadBehavior.Configuration
{
    public class AppConfigSettings : SettingsBase
    {
        private BadBehaviorConfigurationSection config;

        public AppConfigSettings()
        {
            this.config = ConfigurationManager.GetSection("badBehavior")
                as BadBehaviorConfigurationSection
                ?? new BadBehaviorConfigurationSection();
        }

        public override bool AllowRemoteLogViewing { get { return config.AllowRemoteLogViewing; } }
        public override bool Debug { get { return config.Debug; } }
        public override bool OffsiteForms { get { return config.OffsiteForms; } }
        public override bool Strict { get { return config.Strict; } }
        public override string SupportEmail { get { return config.SupportEmail; } }
        public override bool ReverseProxy { get { return config.ReverseProxy; } }

        public override IList<string> ReverseProxyAddresses
        {
            get {
                return
                    config.ReverseProxyAddresses.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        public override string ReverseProxyHeader { get { return config.ReverseProxyHeader; } }

        public override IList<string> WhitelistIPRanges
        {
            get {
                return
                    config.WhiteList.IPRanges.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        public override IList<string> WhitelistUserAgents
        {
            get
            {
                return
                    config.WhiteList.UserAgents.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        public override IList<string> WhitelistUrls
        {
            get
            {
                return
                    config.WhiteList.Urls.OfType<ValueElement>()
                        .Select(x => x.Value).ToList();
            }
        }

        public override bool Httpbl
        {
            get
            {
                return config.Httpbl != null && !String.IsNullOrEmpty(config.Httpbl.Key);
            }
        }

        public override string HttpblKey
        {
            get
            {
                return config.Httpbl.Key;
            }
        }

        public override int HttpblThreatLevel
        {
            get
            {
                return config.Httpbl.ThreatLevel;
            }
        }

        public override int HttpblMaxAge
        {
            get
            {
                return config.Httpbl.MaxAge;
            }
        }

    }
}
