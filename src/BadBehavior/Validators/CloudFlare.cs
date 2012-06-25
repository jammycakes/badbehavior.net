using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Validators
{
    public class CloudFlare : IValidation
    {
        public ValidationResult Validate(Package package)
        {
            return ValidationResult.Continue;
        }
    }
}
