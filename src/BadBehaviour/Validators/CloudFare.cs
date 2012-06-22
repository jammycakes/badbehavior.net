using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehaviour.Validators
{
	public class CloudFare : IValidation
	{
		public ValidationResult Validate(Package package)
		{
			return ValidationResult.Continue;
		}
	}
}
