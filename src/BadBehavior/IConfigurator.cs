using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior
{
    /* ====== IConfigurator interface ====== */

    /// <summary>
    ///  Called to configure Bad Behavior.
    /// </summary>

    public interface IConfigurator
    {
        /// <summary>
        ///  Configures Bad Behavior.
        /// </summary>

        void Configure(BBEngine engine);
    }
}
