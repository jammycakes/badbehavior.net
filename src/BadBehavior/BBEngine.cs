using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BadBehavior.Configuration;
using BadBehavior.Logging;
using BadBehavior.Rules;
using BadBehavior.Util;

namespace BadBehavior
{
    public class BBEngine : BadBehavior.IBBEngine
    {
        public static IBBEngine Instance { get; set; }

        private static readonly Template template
            = Template.FromResource("BadBehavior.response.html");

        static BBEngine()
        {
            BBEngine.Instance = new BBEngine();
        }


        public IList<IRule> Rules { get; private set; }

        public ISettings Settings { get; set; }

        public ILogWriter Logger { get; set; }

        public ILogReader LogReader { get; set; }


        public BBEngine()
        {
            this.Logger = new NullLogWriter();
            this.Settings = ConfigurationManager.GetSection("badBehavior") as ISettings
                ?? new BadBehaviorConfigurationSection();
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
                try {
                    if (rule.Validate(package) == RuleProcessing.Stop) return;
                }
                catch (Exception ex) {
                    /*
                     * An exception in one rule must not bring down the site, or even
                     * stop other rules from running, no matter what the problem is.
                     * The only exception here is BadBehaviorException, which of course
                     * indicates bad behaviour. Log the rest to System.Diagnostics.Trace.
                     */
                    if (ex is BadBehaviorException || this.Settings.Debug)
                        throw;
                    else
                        Trace.TraceWarning(
                            "An error was encountered when attempting to validate this request. "
                            + "Exception details: " + ex.ToString()
                        );
                }
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
            var dict = new Dictionary<string, string>();
            dict["response"] = ex.Error.HttpCode.ToString();
            dict["request_uri"] = ex.Package.Request.RawUrl;
            dict["explanation"] = ex.Error.Explanation;
            dict["support_key"] = BuildSupportKey(ex.Package.OriginatingIP, ex.Error.Code);
            if (email != null) dict["email"] = email;
            return template.Process(dict);
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
            Raise(package, error, false);
        }

        private void Raise(Package package, Error error, bool strict)
        {
            bool thrown = this.Settings.Strict || !strict;
            var ex = new BadBehaviorException(package, error);
            try {
                Logger.Log(new LogEntry(ex, thrown));
                var args = new BadBehaviorEventArgs(package, error);
                OnBadBehavior(args);
            }
            catch (Exception loggingException) {
                /*
                 * An exception when logging or running the event handler needs to
                 * be trapped here, for two reasons. First, logging must not bring
                 * anything to a halt under any circumstances. Second, if we throw
                 * an "uninteresting" (ie non-BB) exception here, it will be
                 * swallowed higher up the call stack in ValidateRequest above,
                 * and the request will be given the all clear when it should have
                 * been rejected.
                 * 
                 * So, Pokémon it and log it to System.Diagnostics.Trace.
                 */
                if (loggingException is BadBehaviorException || this.Settings.Debug)
                    throw;
                else
                    Trace.TraceWarning(
                        "An error was encountered when attempting to validate this request. "
                        + "Exception details: " + loggingException.ToString()
                    );
            }
            if (thrown) {
                throw ex;
            }
        }

        public void RaiseStrict(Package package, Error error)
        {
            Raise(package, error, true);
        }

        public event BadBehaviorEventHandler BadBehavior;

        private void OnBadBehavior(BadBehaviorEventArgs args)
        {
            if (this.BadBehavior != null)
                this.BadBehavior(this, args);
        }
    }
}