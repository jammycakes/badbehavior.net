using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBehavior.Util;
using NUnit.Framework;

namespace BadBehavior.Tests.Util
{
    [TestFixture]
    public class TemplateFixture
    {
        [Test]
        public void CanProcessTags()
        {
            string sTemplate = "Testing the {{template}} tags";
            var args = new Dictionary<string, string>() {
                { "template", "Bad Behavior" }
            };

            var result = new Template(sTemplate).Process(args);
            Assert.AreEqual("Testing the Bad Behavior tags", result);

        }
    }
}
