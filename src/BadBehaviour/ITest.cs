using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BadBehaviour
{
	public interface ITest
	{
		/// <summary>
		///  The Bad Behaviour code returned when this test fails.
		/// </summary>

		string Code { get; }

		/// <summary>
		///  The HTTP response code to be returned to the server when the test fails.
		/// </summary>

		int HttpCode { get; }

		/// <summary>
		///  The explanation to be returned to the server when the test fails.
		/// </summary>

		string Explanation { get; }

		/// <summary>
		///  The message to be entered in the log when the test fails.
		/// </summary>

		string Log { get; }

		/// <summary>
		///  Tests an HTTP request for conformance to this rule.
		/// </summary>
		/// <param name="request">
		///  The HTTP request to be examined. 
		/// </param>
		/// <returns>
		///  true if the request matched the criterion for this test (and therefore failed),
		///  otherwise false.
		/// </returns>

		bool Test(RequestPackage request);
	}
}
