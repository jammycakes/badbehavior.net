using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BadBehavior
{
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

        RuleResult Validate(Package package);
    }
}
