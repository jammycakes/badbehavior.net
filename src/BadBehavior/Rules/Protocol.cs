using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    public class Protocol : IRule
    {
        private void ValidateHttp10(Package package)
        {
            // We should never see Expect: for HTTP/1.0 requests
            if (package.HeadersMixed.ContainsKey("Expect")
                && package.HeadersMixed["Expect"].IndexOf
                    ("100-continue", StringComparison.InvariantCultureIgnoreCase) < 0) {
                        package.Raise(Errors.Http10Expect);
            }
        }

        private void ValidateHttp11(Package package)
        {
            // Is it claiming to be HTTP/1.1?  Then it shouldn't do HTTP/1.0 things
            // Blocks some common corporate proxy servers in strict mode

            if (package.HeadersMixed.ContainsKey("Pragma")
                && package.HeadersMixed["Pragma"].Contains("no-cache")
                && !package.HeadersMixed.ContainsKey("Cache-Control")) {
                    package.RaiseStrict(Errors.Http11Invalid);
            }
        }

        public RuleProcessing Validate(Package package)
        {
            switch (package.Request.ServerVariables["SERVER_PROTOCOL"]) {
                case "HTTP/1.0":
                    ValidateHttp10(package);
                    break;
                case "HTTP/1.1":
                    ValidateHttp11(package);
                    break;
            }

            return RuleProcessing.Continue;
        }
    }
}
