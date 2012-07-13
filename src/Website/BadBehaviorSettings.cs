using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BadBehavior;

namespace Website
{
    public class BadBehaviorSettings : ISettings
    {
        public bool AllowRemoteLogViewing
        {
            get { return false; }
        }

        public bool Debug
        {
            get { return false; }
        }

        public bool OffsiteForms
        {
            get { return false; }
        }

        public bool Strict
        {
            get { return false; }
        }

        public string SupportEmail
        {
            get { return "bad.behavior@example.com"; }
        }

        public bool ReverseProxy
        {
            get { return false; }
        }

        public IList<string> ReverseProxyAddresses
        {
            get { return null; }
        }

        public string ReverseProxyHeader
        {
            get { return null; }
        }

        public IList<string> WhitelistIPRanges
        {
            get { return null; }
        }

        public IList<string> WhitelistUserAgents
        {
            get { return null; }
        }

        public IList<string> WhitelistUrls
        {
            get { return null; }
        }

        public bool Httpbl
        {
            get { return false; }
        }

        public string HttpblKey
        {
            get { return null; }
        }

        public int HttpblThreatLevel
        {
            get { return 25; }
        }

        public int HttpblMaxAge
        {
            get { return 30; }
        }
    }
}