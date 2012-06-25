using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BadBehavior
{
    public class BBEngine
    {
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
        }


        public static BBEngine Instance { get; set; }

        public void ValidateRequest(HttpRequestBase request)
        {
            Validator.Instance.Validate(request);
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

    }
}