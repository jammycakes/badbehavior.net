using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehaviour.Validators
{
	public class MovableType : IValidation
	{
		public ValidationResult Validate(Package request)
		{
			return ValidationResult.Continue;
		}
	}
}
