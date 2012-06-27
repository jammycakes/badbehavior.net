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

        private static readonly Error ERangeHeaderZero = new Error(
            "7ad04a8a", 400, Explanations.RangeHeaderZero,
            "Prohibited header \'Range\' present"
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

            // Range: field exists and begins with 0
            // Real user-agents do not start ranges at 0
            // NOTE: this blocks the whois.sc bot. No big loss.
            // Exceptions: MT (not fixable); LJ (refuses to fix; may be
            // blocked again in the future); Facebook
            if (package.Configuration.Strict && package.Headers.ContainsKey("Range")
                && package.Headers["Range"].Contains("=0-")) {
                if (!(
                    package.Request.UserAgent.StartsWith("MovableType") ||
                    package.Request.UserAgent.StartsWith("URI::Fetch") ||
                    package.Request.UserAgent.StartsWith("php-openid/") ||
                    package.Request.UserAgent.StartsWith("facebookexternalhit")
                )) {
                    package.Raise(this, ERangeHeaderZero);
                }

            }

            return RuleProcessing.Continue;
        }
    }
}
