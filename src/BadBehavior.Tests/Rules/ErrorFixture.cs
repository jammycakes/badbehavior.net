using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBehavior.Rules;
using NUnit.Framework;

namespace BadBehavior.Tests.Rules
{
    [TestFixture]
    public class ErrorFixture
    {
        [Test]
        public void CanLookupError()
        {
            var error = Errors.Lookup("f9f2b8b9");
            Assert.AreSame(Errors.UserAgentMissing, error);
        }

        [Test]
        public void CanLookupNonexistentError()
        {
            var error = Errors.Lookup("kwyjibo!");
            Assert.IsNull(error);
        }
    }
}
