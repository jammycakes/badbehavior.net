using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class SearchEngine : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            return RuleProcessing.Continue;
        }
    }
}
