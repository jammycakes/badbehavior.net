using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BadBehavior.Util
{
    public class Template
    {
        public string Text { get; private set; }

        public Template(string text)
        {
            this.Text = text;
        }

        public static Template FromStream(Stream input)
        {
            using (var reader = new StreamReader(input))
                return new Template(reader.ReadToEnd());
        }

        internal static Template FromResource(string resourceName)
        {
            using (var stream = typeof(Template).Assembly.GetManifestResourceStream(resourceName))
                return FromStream(stream);
        }

        private static readonly Regex reReplaceConditionals
            = new Regex(@"\{\{(.*?)\?\}\}(.*?)\{\{/\1\?\}\}");

        private static string ReplaceConditionals(string tpl, Func<string, string> getter)
        {
            return reReplaceConditionals.Replace(tpl,
                x => getter(x.Groups[1].Value) != null
                    ? x.Groups[2].Value : String.Empty
            );
        }

        private static readonly Regex reReplaceTags = new Regex(@"\{\{(.*?)\}\}");

        private static string ReplaceTags(string tpl, Func<string, string> getter)
        {
            return reReplaceTags.Replace(
                tpl,
                x => HttpUtility.HtmlEncode(getter(x.Groups[1].Value) ?? String.Empty)
            );
        }

        public string Process(IDictionary<string, string> data)
        {
            Func<string, string> getter = x => {
                string result;
                return data.TryGetValue(x, out result) ? result : null;
            };

            string tpl = ReplaceConditionals(this.Text, getter);
            return ReplaceTags(tpl, getter);
        }
    }
}
