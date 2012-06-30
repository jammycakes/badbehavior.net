﻿using System;
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
                package.Raise(this, Errors.TrackbackFromWebBrowser);

            // Proxy servers don't send trackbacks either
            if (package.HeadersMixed.ContainsKey("Via") ||
                package.HeadersMixed.ContainsKey("Max-Forwards") ||
                package.HeadersMixed.ContainsKey("X-Forwarded-For") ||
                package.HeadersMixed.ContainsKey("Client-Ip"))
                package.Raise(this, Errors.TrackbackFromProxyServer);

            // Fake WordPress trackbacks
            // Real ones do not contain Accept:, and have a charset defined
            // Real WP trackbacks may contain Accept: depending on the HTTP
            // transport being used by the sending host
            if (package.Request.UserAgent.Contains("WordPress/"))
                if (!package.Request.ContentType.Contains("charset="))
                    package.Raise(this, Errors.FakeWordPress);
        }

        private void ValidatePost(Package package)
        {
            // MovableType needs specialized screening
            if (package.UserAgentI.Contains("movabletype")) {
                if ("bytes=0-99999".Equals(package.HeadersMixed["Range"]))
                    package.Raise(this, Errors.InvalidRangeHeader);
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
                    package.Raise(this, Errors.Malicious);
        }
    }
}
