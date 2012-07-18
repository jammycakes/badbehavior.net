using System.Linq;
using BadBehavior.Util;

namespace BadBehavior.Rules
{
    public class SearchEngine : IRule
    {
        // NOTE: Put all user agent substrings in lowercase here.
        // 

        private static readonly string[] uaMSNbot =
            { "bingbot", "msnbot", "ms search" };
        private static readonly string[] cidrMSNbot = {
            "207.46.0.0/16", "65.52.0.0/14", "207.68.128.0/18", "207.68.192.0/20",
            "64.4.0.0/18", "157.54.0.0/15", "157.60.0.0/16", "157.56.0.0/14"
        };

        private static readonly string[] uaGooglebot =
            { "googlebot", "mediapartners-google", "google web preview" };
        private static readonly string[] cidrGooglebot = {
            "66.249.64.0/19", "64.233.160.0/19", "72.14.192.0/18", "203.208.32.0/19",
            "74.125.0.0/16", "216.239.32.0/19", "209.85.128.0/17"
        };

        private static readonly string[] uaYahoobot =
            { "yahoo! slurp", "yahoo! searchmonkey" };
        private static readonly string[] cidrYahoobot = { 
            "202.160.176.0/20", "67.195.0.0/16", "203.209.252.0/24",
            "72.30.0.0/16", "98.136.0.0/14", "74.6.0.0/16"
        };

        private void Validate(Package package, string[] ua, string[] cidr, Error error)
        {
            if (ua.Any(x => package.UserAgentI.Contains(x)))
                if (!package.OriginatingIP.MatchCidr(cidr))
                    package.Raise(error);
        }

        public RuleProcessing Validate(Package package)
        {
            this.Validate(package, uaMSNbot, cidrMSNbot, Errors.FakeMSNbot);
            this.Validate(package, uaGooglebot, cidrGooglebot, Errors.FakeGooglebot);
            this.Validate(package, uaYahoobot, cidrYahoobot, Errors.FakeYahoobot);
            return RuleProcessing.Continue;
        }
    }
}
