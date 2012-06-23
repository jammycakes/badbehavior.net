using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Validators
{
    public class WhiteList : IValidation
    {
        public ValidationResult Validate(Package package)
        {
            // NB: return ValidationResult.Stop if this request is whitelisted.
            // Don't throw from this method.
            return ValidationResult.Continue;
        }
    }
}
