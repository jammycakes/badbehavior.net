using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class MiscHeaders : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            if (package.Request.HttpMethod != "POST" && String.IsNullOrEmpty(package.Request.UserAgent))
                package.Raise(this, Errors.UserAgentMissing);

            // Broken spambots send URLs with various invalid characters
            // Some broken browsers send the #vector in the referer field :(
            // Worse yet, some Javascript client-side apps do the same in
            // blatant violation of the protocol and good sense.
            // if (strpos($package['request_uri'], "#") !== FALSE || strpos($package['headers_mixed']['Referer'], "#") !== FALSE) {

            if (package.Configuration.Strict && package.Request.RawUrl.Contains('#'))
                package.Raise(this, Errors.Malicious);

            // A pretty nasty SQL injection attack on IIS servers
            if (package.Request.RawUrl.Contains(";DECLARE%20@"))
                package.Raise(this, Errors.Malicious);

            // Range: field exists and begins with 0
            // Real user-agents do not start ranges at 0
            // NOTE: this blocks the whois.sc bot. No big loss.
            // Exceptions: MT (not fixable); LJ (refuses to fix; may be
            // blocked again in the future); Facebook
            if (package.Configuration.Strict && package.HeadersMixed.ContainsKey("Range")
                && package.HeadersMixed["Range"].Contains("=0-")) {
                if (!(
                    package.Request.UserAgent.StartsWith("MovableType") ||
                    package.Request.UserAgent.StartsWith("URI::Fetch") ||
                    package.Request.UserAgent.StartsWith("php-openid/") ||
                    package.Request.UserAgent.StartsWith("facebookexternalhit")
                )) {
                    package.Raise(this, Errors.RangeHeaderZero);
                }
            }

            // Content-Range is a response header, not a request header
            if (package.HeadersMixed.ContainsKey("Content-Range"))
                package.Raise(this, Errors.InvalidRangeHeader);

            // Lowercase via is used by open proxies/referrer spammers
            // Exceptions: Clearswift uses lowercase via (refuses to fix;
            // may be blocked again in the future)
            if (package.Request.Headers["via"] != null &&
                !package.Request.Headers["via"].Contains("Clearswift") &&
                !package.Request.UserAgent.Contains("CoralWebPrx"))
                package.Raise(this, Errors.InvalidVia);

            // pinappleproxy is used by referrer spammers
            if (package.HeadersMixed.ContainsKey("Via")) {
                string via = package.HeadersMixed["Via"].ToLowerInvariant();
                if (via.Contains("pinappleproxy") || via.Contains("pcnetserver") || via.Contains("invisiware"))
                    package.Raise(this, Errors.BannedProxy);
            }

            return RuleProcessing.Continue;
        }
    }
}
