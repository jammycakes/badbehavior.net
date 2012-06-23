﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Validators
{
    public class Protocol : IValidation
    {
        private static readonly Error EHttp10Expect = new Error(
            "a0105122", 417, Explanations.Http10Expect,
            "Header 'Expect' prohibited; resend without Expect"
        );

        private static readonly Error EHttp11Invalid = new Error(
            "41feed15", 400, Explanations.Http11Invalid,
            "Header 'Pragma' without 'Cache-Control' prohibited for HTTP/1.1 requests"
        );

        private void ValidateHttp10(Package package)
        {
            // We should never see Expect: for HTTP/1.0 requests
            if (package.Headers.ContainsKey("Expect")
                && package.Headers["Expect"].IndexOf
                    ("100-continue", StringComparison.InvariantCultureIgnoreCase) < 0) {
                throw new BadBehaviorException(this, package.Request, EHttp10Expect);
            }
        }

        private void ValidateHttp11(Package package)
        {
            // Is it claiming to be HTTP/1.1?  Then it shouldn't do HTTP/1.0 things
            // Blocks some common corporate proxy servers in strict mode

            if (package.Headers.ContainsKey("Pragma")
                && package.Headers["Pragma"].Contains("no-cache")
                && !package.Headers.ContainsKey("Cache-Control")) {
                throw new BadBehaviorException(this, package.Request, EHttp11Invalid);
            }
        }

        public ValidationResult Validate(Package package)
        {
            switch (package.Request.ServerVariables["HTTP_PROTOCOL"]) {
                case "HTTP/1.0":
                    ValidateHttp10(package);
                    break;
                case "HTTP/1.1":
                    if (package.Configuration.Strict) {
                        ValidateHttp11(package);
                    }
                    break;
            }

            return ValidationResult.Continue;
        }
    }
}
