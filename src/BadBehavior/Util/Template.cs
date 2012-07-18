using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace BadBehavior.Util
{
    /// <summary>
    ///  A very basic lightweight templating engine.
    /// </summary>
    /// <remarks>
    ///  This templating engine offers a subset of the mustache syntax.
    ///  It gives you {{template tags}} and {{conditionals?}} {{/conditionals?}}
    ///  and that's about it.
    /// </remarks>

    public class Template
    {
        /// <summary>
        ///  Gets the text of the template.
        /// </summary>

        public string Text { get; private set; }

        /// <summary>
        ///  Creates a new template from the given string.
        /// </summary>
        /// <param name="text"></param>

        public Template(string text)
        {
            this.Text = text;
        }

        /// <summary>
        ///  Loads a template from a stream.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public static Template FromStream(Stream input)
        {
            using (var reader = new StreamReader(input))
                return new Template(reader.ReadToEnd());
        }

        /// <summary>
        ///  Loads a template from a resource in this assembly.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>

        internal static Template FromResource(string resourceName)
        {
            using (var stream = typeof(Template).Assembly.GetManifestResourceStream(resourceName))
                return FromStream(stream);
        }

        private static readonly Regex reReplaceConditionals
            = new Regex(@"\{\{(.*?)\?\}\}(.*?)\{\{/\1\?\}\}", RegexOptions.Singleline);

        private static string ReplaceConditionals(string tpl, Func<string, string> getter)
        {
            return reReplaceConditionals.Replace(tpl,
                x => getter(x.Groups[1].Value) != null
                    ? x.Groups[2].Value : String.Empty
            );
        }

        private static readonly Regex reReplaceLineBreaks = new Regex(@"\r?\n", RegexOptions.Singleline);

        private static string HtmlEncode(string str)
        {
            var result = HttpUtility.HtmlEncode(str);
            return reReplaceLineBreaks.Replace(result, "<br />");
        }

        private static readonly Regex reReplaceTags = new Regex(@"(\{{2,3})(.*?)(\}{2,3})");

        private static string ReplaceTags(string tpl, Func<string, string> getter)
        {
            return reReplaceTags.Replace(
                tpl,
                x => {
                    string key = x.Groups[2].Value;
                    if (x.Groups[1].Value.Length == 3 && x.Groups[3].Value.Length == 3) {
                        return getter(key) ?? String.Empty;
                    }
                    else if (x.Groups[1].Value.Length == 2 && x.Groups[3].Value.Length == 2) {
                        return HtmlEncode(getter(key) ?? String.Empty);
                    }
                    else {
                        return String.Empty; // invalid tag
                    }
                }
            );
        }

        /// <summary>
        ///  Processes the template with the given data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

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
