using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BadBehaviour
{
	public interface IValidation
	{
		/// <summary>
		///  Tests an HTTP request for conformance to this rule.
		/// </summary>
		/// <param name="package">
		///  The HTTP request to be examined. 
		/// </param>
		/// <exception cref="BadBehaviourException">
		///  Thrown when the request fails to validate against this test.
		/// </exception>

		ValidationResult Validate(Package package);
	}
}
