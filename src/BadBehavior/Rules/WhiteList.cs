using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class WhiteList : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            // NB: return RuleProcessing.Stop if this request is whitelisted.
            // Don't throw from this method.

            if (package.OriginatingIP.MatchCidr(package.Configuration.WhitelistIPRanges.ToArray()))
                return RuleProcessing.Stop;

            if (package.Configuration.WhitelistUserAgents.Any(x => x == package.Request.UserAgent))
                return RuleProcessing.Stop;

            string url = package.Request.Path;
            if (package.Configuration.WhitelistUrls.Any(x => url.StartsWith(x)))
                return RuleProcessing.Stop;

            return RuleProcessing.Continue;
        }
    }
}
