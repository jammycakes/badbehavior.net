using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior
{
    /* ====== IConfigurator interface ====== */

    /// <summary>
    ///  Implement this interface in your web application to create a class which will be
    ///  loaded in at application startup to configure Bad Behavior .NET.
    /// </summary>

    public interface IConfigurator
    {
        /// <summary>
        ///  Configures Bad Behavior.
        /// </summary>
        /// <param name="engine">
        ///  The <see cref="BBEngine"/> instance to configure.
        /// </param>

        void Configure(BBEngine engine);
    }
}
