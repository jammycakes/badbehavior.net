using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BadBehavior;

namespace Website
{
    public class BadBehaviorSettings : SettingsBase
    {
        public override bool AllowRemoteLogViewing
        {
            get { return false; }
        }

        public override bool Debug
        {
            get { return false; }
        }

        public override bool OffsiteForms
        {
            get { return false; }
        }

        public override bool Strict
        {
            get { return false; }
        }

        public override string SupportEmail
        {
            get { return "bad.behavior@example.com"; }
        }

        public override bool ReverseProxy
        {
            get { return false; }
        }

        public override IList<string> ReverseProxyAddresses
        {
            get { return null; }
        }

        public override string ReverseProxyHeader
        {
            get { return null; }
        }

        public override IList<string> WhitelistIPRanges
        {
            get { return null; }
        }

        public override IList<string> WhitelistUserAgents
        {
            get { return null; }
        }

        public override IList<string> WhitelistUrls
        {
            get { return null; }
        }

        public override bool Httpbl
        {
            get { return false; }
        }

        public override string HttpblKey
        {
            get { return null; }
        }

        public override int HttpblThreatLevel
        {
            get { return 25; }
        }

        public override int HttpblMaxAge
        {
            get { return 30; }
        }
    }
}