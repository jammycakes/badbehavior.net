using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class Cookies : IRule
    {
        public RuleResult Validate(Package package)
        {
            return RuleResult.Continue;
        }
    }
}
