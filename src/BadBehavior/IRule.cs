namespace BadBehavior
{
    /* ====== IRule interface ====== */

    /// <summary>
    ///  Implements a rule which requests must pass.
    /// </summary>

    public interface IRule
    {
        /// <summary>
        ///  Tests an HTTP request for conformance to this rule.
        /// </summary>
        /// <param name="package">
        ///  The HTTP request to be examined. 
        /// </param>
        /// <exception cref="BadBehaviorException">
        ///  Thrown when the request fails to validate against this test.
        /// </exception>
        /// <remarks>
        ///  <para>
        ///   When a request fails to validate, this method MUST throw an exception
        ///   of type <see cref="BadBehaviorException" />. When it validates, it
        ///   should return a result of RuleProcessing.Continue.
        ///  </para>
        ///  <para>
        ///   A result of RuleProcessing.Stop indicates that the request should be
        ///   considered to have passed and no further checks should be carried out.
        ///   This is used for whitelisting.
        ///  </para>
        /// </remarks>

        RuleProcessing Validate(Package package);
    }
}
