﻿using BadBehavior.Util;

namespace BadBehavior.Rules
{
    public class CloudFlare : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            if (package.HeadersMixed.ContainsKey("Cf-Connecting-Ip")) {
                // Not implemented in BB reference.
            }

            return RuleProcessing.Continue;
        }
    }
}
