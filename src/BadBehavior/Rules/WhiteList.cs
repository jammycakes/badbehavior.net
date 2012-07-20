using System.Linq;
using BadBehavior.Util;

namespace BadBehavior.Rules
{
    public class WhiteList : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            // NB: return RuleProcessing.Stop if this request is whitelisted.
            // Don't throw from this method.

            if (package.OriginatingIP.MatchCidr(package.Settings.WhitelistIPRanges.ToArray()))
                return RuleProcessing.Stop;

            if (package.Settings.WhitelistUserAgents.Any(x => x == package.Request.UserAgent))
                return RuleProcessing.Stop;

            string url = package.Request.Path;
            if (package.Settings.WhitelistUrls.Any(x => url.StartsWith(x)))
                return RuleProcessing.Stop;

            return RuleProcessing.Continue;
        }
    }
}
