using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior
{
	[Serializable]
	public class Error
	{
		/// <summary>
		///  The Bad Behaviour code returned when this test fails.
		/// </summary>

		public string Code { get; private set; }

		/// <summary>
		///  The HTTP response code to be returned to the server when the test fails.
		/// </summary>

		public int HttpCode { get; private set; }

		/// <summary>
		///  The explanation to be returned to the server when the test fails.
		/// </summary>

		public string Explanation { get; private set; }

		/// <summary>
		///  The message that should be saved into the application's log.
		/// </summary>

		public string Log { get; private set; }


		public Error(string code, int httpCode, string explanation, string log)
		{
			this.Code = code;
			this.HttpCode = httpCode;
			this.Explanation = explanation;
			this.Log = log;
		}
	}
}
