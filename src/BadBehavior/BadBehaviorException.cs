using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace BadBehavior
{
	/// <summary>
	///  The exception that is thrown when Bad Behaviour fails to validate a request.
	/// </summary>

	[Serializable]
	public class BadBehaviorException : Exception
	{
		/// <summary>
		///  Gets the test which failed validation.
		/// </summary>

		public IValidation FailedTest { get; private set; }

		/// <summary>
		///  Gets the request which failed to validate.
		/// </summary>

		public HttpRequestBase Request { get; private set; }

		/// <summary>
		///  The <see cref="Error" /> instance indicating which error condition was triggered.
		/// </summary>

		public Error Error { get; private set; }


		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviorException" /> class.
		/// </summary>

		public BadBehaviorException() : base()
		{ }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviorException" /> class
		///  with a specific message.
		/// </summary>

		public BadBehaviorException(string message) : base(message)
		{ }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviorException" /> class
		///  with a specific message and inner exception.
		/// </summary>

		public BadBehaviorException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviorException" /> class
		///  recording the request that failed and the test that triggered the failure.
		/// </summary>
		/// <remarks>
		///  The exception message uses the Log property in the <see cref="IValidation"/> instance.
		///  This should not be shown to the user.
		/// </remarks>

		public BadBehaviorException(IValidation failedTest, HttpRequestBase request, Error error)
			: base(error.Log)
		{
			this.Request = request;
			this.FailedTest = failedTest;
			this.Error = error;
		}

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviorException"/> class with
		///  serialised data.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="info"></param>

		protected BadBehaviorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
