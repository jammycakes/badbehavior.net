using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class MiscHeaders : IRule
    {
        private static readonly Error EUserAgentMissing = new Error(
            "f9f2b8b9", 403, Explanations.UserAgentMissing,
            "A User-Agent is required but none was provided."
        );

        private static readonly Error EMalicious = new Error(
            "dfd9b1ad", 403, Explanations.Malicious,
            "Request contained a malicious JavaScript or SQL injection attack."
        );

        public RuleProcessing Validate(Package package)
        {
            if (package.Request.HttpMethod != "POST" && String.IsNullOrEmpty(package.Request.UserAgent))
                package.Raise(this, EUserAgentMissing);

            // Broken spambots send URLs with various invalid characters
            // Some broken browsers send the #vector in the referer field :(
            // Worse yet, some Javascript client-side apps do the same in
            // blatant violation of the protocol and good sense.
            // if (strpos($package['request_uri'], "#") !== FALSE || strpos($package['headers_mixed']['Referer'], "#") !== FALSE) {

            if (package.Configuration.Strict && package.Request.RawUrl.Contains('#'))
                package.Raise(this, EMalicious);

            // A pretty nasty SQL injection attack on IIS servers
            if (package.Request.RawUrl.Contains(";DECLARE%20@"))
                package.Raise(this, EMalicious);

            return RuleProcessing.Continue;
        }
    }
}
