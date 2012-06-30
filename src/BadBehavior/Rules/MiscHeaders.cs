using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadBehavior.Rules
{
    public class MiscHeaders : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            if (package.Request.HttpMethod != "POST" && String.IsNullOrEmpty(package.Request.UserAgent))
                package.Raise(Errors.UserAgentMissing);

            // Broken spambots send URLs with various invalid characters
            // Some broken browsers send the #vector in the referer field :(
            // Worse yet, some Javascript client-side apps do the same in
            // blatant violation of the protocol and good sense.
            // if (strpos($package['request_uri'], "#") !== FALSE || strpos($package['headers_mixed']['Referer'], "#") !== FALSE) {

            if (package.Settings.Strict && package.Request.RawUrl.Contains('#'))
                package.Raise(Errors.Malicious);

            // A pretty nasty SQL injection attack on IIS servers
            if (package.Request.RawUrl.Contains(";DECLARE%20@"))
                package.Raise(Errors.Malicious);

            // Range: field exists and begins with 0
            // Real user-agents do not start ranges at 0
            // NOTE: this blocks the whois.sc bot. No big loss.
            // Exceptions: MT (not fixable); LJ (refuses to fix; may be
            // blocked again in the future); Facebook
            if (package.Settings.Strict && package.HeadersMixed.ContainsKey("Range")
                && package.HeadersMixed["Range"].Contains("=0-")) {
                if (!(
                    package.Request.UserAgent.StartsWith("MovableType") ||
                    package.Request.UserAgent.StartsWith("URI::Fetch") ||
                    package.Request.UserAgent.StartsWith("php-openid/") ||
                    package.Request.UserAgent.StartsWith("facebookexternalhit")
                )) {
                    package.Raise(Errors.RangeHeaderZero);
                }
            }

            // Content-Range is a response header, not a request header
            if (package.HeadersMixed.ContainsKey("Content-Range"))
                package.Raise(Errors.InvalidRangeHeader);

            // Lowercase via is used by open proxies/referrer spammers
            // Exceptions: Clearswift uses lowercase via (refuses to fix;
            // may be blocked again in the future)
            if (package.Request.Headers["via"] != null &&
                !package.Request.Headers["via"].Contains("Clearswift") &&
                !package.Request.UserAgent.Contains("CoralWebPrx"))
                package.Raise(Errors.InvalidVia);

            // pinappleproxy is used by referrer spammers
            if (package.HeadersMixed.ContainsKey("Via")) {
                string via = package.HeadersMixed["Via"].ToLowerInvariant();
                if (via.Contains("pinappleproxy") || via.Contains("pcnetserver") || via.Contains("invisiware"))
                    package.Raise(Errors.BannedProxy);
            }

            // TE: if present must have Connection: TE
            // RFC 2616 14.39
            // Blocks Microsoft ISA Server 2004 in strict mode. Contact Microsoft
            // to obtain a hotfix.
            if (package.Settings.Strict && package.HeadersMixed.ContainsKey("Te")) {
                if (Regex.Match(package.HeadersMixed["Connection"], @"\bTE\b").Success)
                    package.Raise(Errors.TeWithoutConnectionTe);
            }

            if (package.HeadersMixed.ContainsKey("Connection")) {
                // Connection: keep-alive and close are mutually exclusive.
                // Keep-Alive shouldn't appear twice, neither should Close.
                var connection = package.HeadersMixed.GetValues("Connection");
                var keepAlives = connection.Sum
                    (x => Regex.Matches(x, @"\bKeep-Alive\b", RegexOptions.IgnoreCase).Count);
                var closes = connection.Sum
                    (x => Regex.Matches(x, @"\bClose\b", RegexOptions.IgnoreCase).Count);
                if (keepAlives + closes > 1)
                    package.Raise(Errors.InvalidConnectionHeader);

                // Keep-Alive format in RFC 2068; some bots mangle these headers
                if (connection.Any(x => x.IndexOf
                    ("Keep-Alive: ", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    package.Raise(Errors.MaliciousConnectionHeader);
            }

            // Headers which are not seen from normal user agents; only malicious bots
            if (package.HeadersMixed.ContainsKey("X-Aaaaaaaaaaaa") ||
                package.HeadersMixed.ContainsKey("X-Aaaaaaaaaa"))
                package.Raise(Errors.ProhibitedHeader);

            // Proxy-Connection does not exist and should never be seen in the wild
            // http://lists.w3.org/Archives/Public/ietf-http-wg-old/1999JanApr/0032.html
            // http://lists.w3.org/Archives/Public/ietf-http-wg-old/1999JanApr/0040.html
            if (package.Settings.Strict && package.HeadersMixed.ContainsKey("Proxy-Connection"))
                package.Raise(Errors.ProxyConnectionHeaderPresent);

            if (package.HeadersMixed.ContainsKey("Referer")) {
                string referer = package.HeadersMixed["Referer"];
                // Referer, if it exists, must not be blank
                if (referer == String.Empty)
                    package.Raise(Errors.BlankReferer);

                // Referer, if it exists, must contain a :
                // While a relative URL is technically valid in Referer, all known
                // legitimate user-agents send an absolute URL
                if (!referer.Contains(":"))
                    package.Raise(Errors.CorruptReferer);
            }

            return RuleProcessing.Continue;
        }
    }
}
