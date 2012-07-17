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
                { "template", "<strong>Bad Behavior</strong>" }
            };

            var result = new Template(sTemplate).Process(args);
            Assert.AreEqual("Testing the &lt;strong&gt;Bad Behavior&lt;/strong&gt; tags", result);
        }

        [Test]
        public void CanProcessOptionalBlocks()
        {
            string sTemplate = "**{{template?}}Testing\nthe {{template}} tags{{/template?}}**";
            var args = new Dictionary<string, string>();
            var tpl = new Template(sTemplate);
            Assert.AreEqual("****", tpl.Process(args));
            args.Add("template", "Bad Behavior");
            Assert.AreEqual("**Testing\nthe Bad Behavior tags**", tpl.Process(args));
        }

        [Test]
        public void CanProcessRawTags()
        {
            string sTemplate = "Testing the {{{template}}} tags";
            var args = new Dictionary<string, string>() {
                { "template", "<strong>Bad Behavior</strong>" }
            };
            var result = new Template(sTemplate).Process(args);
            Assert.AreEqual("Testing the <strong>Bad Behavior</strong> tags", result);
        }

        [Test]
        public void CanProcessLineBreaks()
        {
            string sLine = "Unix\nWindows\r\netc";
            string sTemplate = "{{test}}";
            var result = new Template(sTemplate).Process(new Dictionary<string, string>() {
                { "test", sLine }
            });
            Assert.AreEqual("Unix<br />Windows<br />etc", result);
        }
    }
}
