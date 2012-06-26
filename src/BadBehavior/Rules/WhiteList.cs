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
            // NB: return ValidationResult.Stop if this request is whitelisted.
            // Don't throw from this method.
            return RuleProcessing.Continue;
        }
    }
}
