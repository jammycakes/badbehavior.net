using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BadBehavior.Rules;

namespace BadBehavior
{
    public class BBEngine : BadBehavior.IBBEngine
    {
        public static IBBEngine Instance { get; set; }

        private static readonly string template;
        private static readonly string templateNoEmail;

        static BBEngine()
        {
            using (var stream = typeof(BadBehaviorModule).Assembly.GetManifestResourceStream
                (typeof(BadBehaviorModule).Namespace + ".response.html"))
            using (var reader = new StreamReader(stream)) {
                string tpl = reader.ReadToEnd();
                templateNoEmail =
                    new Regex(@"\{\{email\?\}\}.*?\{\{/email\?\}\}", RegexOptions.Singleline)
                    .Replace(tpl, String.Empty);
                template = new Regex(@"\{\{/?email\?\}\}", RegexOptions.Singleline)
                    .Replace(tpl, String.Empty);
            }

            BBEngine.Instance = new BBEngine();
        }


        public IList<IRule> Rules { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public BBEngine()
        {
            this.Configuration = new Configuration();
            this.Rules = new IRule[] {
                new CloudFlare(),
                new WhiteList(),
                new BlackList(),
                new BlackHole(),
                new Protocol(),
                new Cookies(),
                new MiscHeaders(),
                new SearchEngine(),
                new Browser(),
                new Post()
            }.ToList();
        }

        public BBEngine(IConfiguration configuration, params IRule[] rules)
        {
            this.Configuration = configuration;
            this.Rules = rules.ToList();
        }


        public void ValidateRequest(HttpRequestBase request)
        {
            var package = new Package(request, this);

            foreach (var rule in this.Rules) {
                if (rule.Validate(package) == RuleProcessing.Stop) return;
            }
        }

        public void HandleError(HttpApplication context, BadBehaviorException ex)
        {
            string content = GetResponseContent(ex);
            context.Response.StatusCode = ex.Error.HttpCode;
            context.Response.StatusDescription = "Bad Behavior";
            context.Response.AddHeader("Status", "Bad Behavior");
            context.Response.ContentType = "text/html";
            context.Response.Write(content);
            context.Server.ClearError();
            context.CompleteRequest();
        }

        public static string GetResponseContent(BadBehaviorException ex)
        {
            string email = ex.Package.Configuration.SupportEmail;
            string tpl = email != null ? template : templateNoEmail;
            var dict = new Dictionary<string, string>();
            dict["response"] = ex.Error.HttpCode.ToString();
            dict["request_uri"] = ex.Package.Request.RawUrl;
            dict["explanation"] = ex.Error.Explanation;
            dict["support_key"] = BuildSupportKey(ex.Package.OriginatingIP, ex.Error.Code);
            if (email != null) dict["email"] = email;
            string content = Regex.Replace(tpl, @"\{\{(.*?)\}\}", m => {
                string key = m.Groups[1].Value;
                if (dict.ContainsKey(key))
                    return HttpUtility.HtmlEncode(dict[key]);
                else
                    return m.Value;
            });
            return content;
        }

        public static string BuildSupportKey(IPAddress ipAddress, string errorCode)
        {
            string s = String.Concat
                (ipAddress.GetAddressBytes().Select(x => x.ToString("x2")).ToArray());
            s = s.Substring(0, 4) + "-" + s.Substring(4) + "-" +
                errorCode.Substring(0, 4) + "-" + errorCode.Substring(4);
            return s.ToLowerInvariant();
        }

        public void Raise(IRule validation, Package package, Error error)
        {
            var args = new BadBehaviorEventArgs(validation, package, error);
            OnBadBehavior(args);
            throw new BadBehaviorException(validation, package, error);
        }

        public event BadBehaviorEventHandler BadBehavior;

        private void OnBadBehavior(BadBehaviorEventArgs args)
        {
            if (this.BadBehavior != null)
                this.BadBehavior(this, args);
        }
    }
}