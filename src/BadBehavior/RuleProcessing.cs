﻿namespace BadBehavior
{
    /// <summary>
    ///  Used to indicate whether we should continue processing more rules
    ///  or stop here.
    /// </summary>

    public enum RuleProcessing
    {
        /// <summary>
        ///  Indicates that we should continue with the rest of the checks.
        /// </summary>

        Continue,

        /// <summary>
        ///  Indicates that we should stop checking. This is used for whitelisting.
        /// </summary>

        Stop
    }
}
