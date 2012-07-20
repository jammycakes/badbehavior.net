using System;

namespace BadBehavior
{
    /* ====== BadBehaviorEventArgs class ====== */

    /// <summary>
    ///  Provides data for the <see cref="BadBehaviorEventHandler"/> event.
    /// </summary>

    public class BadBehaviorEventArgs : EventArgs
    {
        /// <summary>
        ///  The <see cref="Error"/> condition which triggered the failure.
        /// </summary>

        public Error Error { get; private set; }

        /// <summary>
        ///  The <see cref="Package"/> containing the request data for the failed request.
        /// </summary>

        public Package Package { get; private set; }

        /// <summary>
        ///  Creates a new instance of the <see cref="BadBehaviorEventArgs"/> class from a
        ///  <see cref="BadBehaviorException"/> condition.
        /// </summary>
        /// <param name="ex">
        ///  The exception which was raised when the test failed.
        /// </param>

        public BadBehaviorEventArgs(BadBehaviorException ex)
        {
            this.Error = ex.Error;
            this.Package = ex.Package;
        }

        /// <summary>
        ///  Creates a new instance of the <see cref="BadBehaviorEventArgs"/> class from a
        ///  <see cref="Package"/> instance and an <see cref="Error"/> instance.
        /// </summary>
        /// <param name="package">
        ///  The package containing request data for the failed request.
        /// </param>
        /// <param name="error">
        ///  The error condition which triggered the failure.
        /// </param>

        public BadBehaviorEventArgs(Package package, Error error)
        {
            this.Package = package;
            this.Error = error;
        }
    }

    /// <summary>
    ///  Occurs when a request has been flagged by Bad Behavior.
    /// </summary>
    /// <param name="sender">
    ///  The source of the event.
    /// </param>
    /// <param name="e">
    ///  A <see cref="BadBehaviorEventArgs"/> instance containing event data.
    /// </param>

    public delegate void BadBehaviorEventHandler(object sender, BadBehaviorEventArgs e);
}
