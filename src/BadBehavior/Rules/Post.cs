using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class Post : IRule
    {
        public RuleProcessing Validate(Package package)
        {
            if (package.Request.RequestType.Equals("POST", StringComparison.InvariantCultureIgnoreCase)) {
                VerifyPost(package);
            }

            return RuleProcessing.Continue;
        }

        private void VerifyTrackbacks(Package package)
        {
        }

        private void VerifyPost(Package package)
        {
            // MovableType needs specialized screening
            if (package.UserAgentI.Contains("movabletype")) {
                if ("bytes=0-99999".Equals(package.HeadersMixed["Range"]))
                    package.Raise(this, Errors.InvalidRangeHeader);
            }

            // Trackbacks need special screening
            if (package.Request.Form.ContainsKey("title") &&
                package.Request.Form.ContainsKey("url") &&
                package.Request.Form.ContainsKey("blog_name")) {
                VerifyTrackbacks(package);
            }

            // Catch a few completely broken spambots
            foreach (string key in package.Request.Form.Keys)
                if (key.Contains("	document.write"))
                    package.Raise(this, Errors.Malicious);
        }
    }
}
