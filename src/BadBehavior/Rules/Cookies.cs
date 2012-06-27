using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class Cookies : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            // Enforce RFC 2965 sec 3.3.5 and 9.1
            // Bots wanting new-style cookies should send Cookie2
            // FIXME: Amazon Kindle is broken; Amazon has been notified 9/24/08
            if (package.Headers.ContainsKey("Cookie") &&
                package.Headers["Cookie"].Contains("$Version=0") &&
                !package.Headers.ContainsKey("Cookie2") &&
                !package.Request.UserAgent.Contains("Kindle/")) {
                package.Raise(this, Errors.EInvalidCookies);
            }

            return RuleProcessing.Continue;
        }
    }
}
