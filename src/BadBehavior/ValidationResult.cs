using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior
{
    public enum ValidationResult
    {
        /// <summary>
        ///  Indicates that we should continue with the rest of the checks.
        /// </summary>

        Continue,

        /// <summary>
        ///  Indicates that we should stop checking.
        /// </summary>

        Stop
    }
}
