using System;

namespace BadBehavior
{
    /* ====== Error class ====== */

    /// <summary>
    ///  Contains generic information about a class of error conditions.
    /// </summary>
    /// <remarks>
    ///  The <see cref="Error"/> class contains generic information that applies
    ///  to all errors of this class. It includes a support code, the HTTP response
    ///  code to return, an explanation to display to the user, and an explanation
    ///  to record in the logs.
    /// </remarks>

    [Serializable]
    public class Error
    {
        /// <summary>
        ///  The Bad Behavior code returned when this test fails.
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

        /// <summary>
        ///  Creates a new instance of the <see cref="Error"/> class.
        /// </summary>
        /// <param name="code">
        ///  The Bad Behavior support code.
        /// </param>
        /// <param name="httpCode">
        ///  The HTTP response code.
        /// </param>
        /// <param name="explanation">
        ///  The explanation to show to the user.
        /// </param>
        /// <param name="log">
        ///  The message to show in the logs.
        /// </param>

        public Error(string code, int httpCode, string explanation, string log)
        {
            this.Code = code;
            this.HttpCode = httpCode;
            this.Explanation = explanation;
            this.Log = log;
        }
    }
}
