namespace BadBehavior
{
    /* ====== RuleProcessing enumeration ====== */

    /// <summary>
    ///  Returned by an <see cref="IRule"/> instance to indicate whether we should continue
    ///  processing other rules, or stop checking here.
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
