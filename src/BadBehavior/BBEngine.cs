using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BadBehavior.Logging;
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

        public ISettings Settings { get; private set; }

        public ILogWriter Logger { get; set; }


        public BBEngine()
        {
            this.Logger = new NullLogWriter();
            this.Settings = ConfigurationManager.GetSection("badBehavior") as ISettings;
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

        public BBEngine(ISettings settings, params IRule[] rules)
        {
            this.Logger = new NullLogWriter();
            this.Settings = settings;
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
            string email = ex.Package.Settings.SupportEmail;
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
            string s = ipAddress.AddressFamily == AddressFamily.InterNetwork
                ? String.Concat(ipAddress.GetAddressBytes().Select(x => x.ToString("x2")).ToArray())
                : String.Empty;
            s += errorCode;
            var sb = new StringBuilder(s);
            for (int i = ((sb.Length - 1) / 4) * 4; i > 0; i -= 4)
                sb.Insert(i, '-');
            return sb.ToString().ToLowerInvariant();
        }

        public void Raise(Package package, Error error)
        {
            var ex = new BadBehaviorException(package, error);
            Logger.Log(new LogEntry(ex));
            var args = new BadBehaviorEventArgs(package, error);
            OnBadBehavior(args);
            throw ex;
        }

        public event BadBehaviorEventHandler BadBehavior;

        private void OnBadBehavior(BadBehaviorEventArgs args)
        {
            if (this.BadBehavior != null)
                this.BadBehavior(this, args);
        }
    }
}