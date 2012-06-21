using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace BadBehaviour
{
	/// <summary>
	///  The exception that is thrown when Bad Behaviour fails to validate a request.
	/// </summary>

	[Serializable]
	public class BadBehaviourException : Exception
	{
		/// <summary>
		///  Gets the test which failed validation.
		/// </summary>

		public ITest FailedTest { get; private set; }

		/// <summary>
		///  Gets the request which failed to validate.
		/// </summary>

		public HttpRequestBase Request { get; private set; }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviourException" /> class.
		/// </summary>

		public BadBehaviourException() : base()
		{ }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviourException" /> class
		///  with a specific message.
		/// </summary>

		public BadBehaviourException(string message) : base(message)
		{ }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviourException" /> class
		///  with a specific message and inner exception.
		/// </summary>

		public BadBehaviourException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviourException" /> class
		///  recording the request that failed and the test that triggered the failure.
		/// </summary>
		/// <remarks>
		///  The exception message uses the Log property in the <see cref="ITest"/> instance.
		///  This should not be shown to the user.
		/// </remarks>

		public BadBehaviourException(ITest failedTest, HttpRequestBase request)
			: base(failedTest.Log)
		{
			this.Request = request;
			this.FailedTest = failedTest;
		}

		/// <summary>
		///  Creates a new instance of the <see cref="BadBehaviourException"/> class with
		///  serialised data.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="info"></param>

		protected BadBehaviourException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
