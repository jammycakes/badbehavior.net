using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        private readonly Regex reReplaceTags = new Regex(@"\{\{(.*?)\}\}");

        private string ReplaceTags(Func<string, string> getter)
        {
            return reReplaceTags.Replace
                (this.Text, x => getter(x.Groups[1].Value) ?? String.Empty);
        }

        public string Process(IDictionary<string, string> data)
        {
            Func<string, string> getter = x => {
                string result;
                return data.TryGetValue(x, out result) ? result : null;
            };

            return ReplaceTags(getter);
        }
    }
}
