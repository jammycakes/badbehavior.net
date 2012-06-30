using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class Post : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            if (package.Request.RequestType.Equals("POST", StringComparison.InvariantCultureIgnoreCase)) {
                ValidatePost(package);
            }

            return RuleProcessing.Continue;
        }

        private void ValidateTrackbacks(Package package)
        {
            // Web browsers don't send trackbacks
            if (package.IsBrowser)
                package.Raise(Errors.TrackbackFromWebBrowser);

            // Proxy servers don't send trackbacks either
            if (package.HeadersMixed.ContainsKey("Via") ||
                package.HeadersMixed.ContainsKey("Max-Forwards") ||
                package.HeadersMixed.ContainsKey("X-Forwarded-For") ||
                package.HeadersMixed.ContainsKey("Client-Ip"))
                package.Raise(Errors.TrackbackFromProxyServer);

            // Fake WordPress trackbacks
            // Real ones do not contain Accept:, and have a charset defined
            // Real WP trackbacks may contain Accept: depending on the HTTP
            // transport being used by the sending host
            if (package.Request.UserAgent.Contains("WordPress/"))
                if (!package.Request.ContentType.Contains("charset="))
                    package.Raise(Errors.FakeWordPress);
        }

        private void ValidatePost(Package package)
        {
            // MovableType needs specialized screening
            if (package.UserAgentI.Contains("movabletype")) {
                if ("bytes=0-99999".Equals(package.HeadersMixed["Range"]))
                    package.Raise(Errors.InvalidRangeHeader);
            }

            // Trackbacks need special screening
            if (package.Request.Form.ContainsKey("title") &&
                package.Request.Form.ContainsKey("url") &&
                package.Request.Form.ContainsKey("blog_name")) {
                ValidateTrackbacks(package);
            }

            // Catch a few completely broken spambots
            foreach (string key in package.Request.Form.Keys)
                if (key.Contains("	document.write"))
                    package.Raise(Errors.Malicious);

            // If Referer exists, it should refer to a page on our site
            if (package.Settings.OffsiteForms && package.Request.UrlReferrer != null) {
                string host = package.Request.Url.Host;
                string referrer = package.Request.UrlReferrer.Host;
                if(referrer.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
                    referrer = referrer.Substring(4);
                if(host.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
                    host = host.Substring(4);
                host = "." + host;
                referrer = "." + referrer;
                if (!referrer.Equals(host, StringComparison.InvariantCultureIgnoreCase))
                    package.Raise(Errors.NotSameOrigin);
            }
        }
    }
}
