using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehaviour.Validators
{
	public class Cookies : IValidation
	{
		public ValidationResult Validate(RequestPackage request)
		{
			return ValidationResult.Continue;
		}
	}
}
