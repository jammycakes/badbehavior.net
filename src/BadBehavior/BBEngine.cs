using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using BadBehavior.Configuration;
using BadBehavior.Logging;
using BadBehavior.Logging.SqlServer;
using BadBehavior.Rules;
using BadBehavior.Util;

namespace BadBehavior
{
    /* ====== BBEngine class ====== */

    /// <summary>
    ///  The "guts" of Bad Behavior, where requests are vetted.
    /// </summary>

    public class BBEngine
    {
        /// <summary>
        ///  The singleton instance of <see cref="BBEngine"/>, set up with
        ///  all the rules and configuration settings used to verify requests.
        /// </summary>

        public static BBEngine Instance { get; set; }

        private static readonly Template template
            = Template.FromResource("BadBehavior.response.html");

        static BBEngine()
        {
            BBEngine.Instance = new BBEngine();
        }

        /// <summary>
        ///  A list of <see cref="IRule" /> instances which will vet the requests.
        /// </summary>

        public IList<IRule> Rules { get; private set; }

        /// <summary>
        ///  A <see cref="Settings"/> instance containing the settings for
        ///  Bad Behavior .NET.
        /// </summary>

        public Settings Settings { get; private set; }

        /// <summary>
        ///  An <see cref="ILogger"/> instance used to log bad and suspicious requests.
        /// </summary>
        /// <remarks>
        ///  By default, this is a <see cref="NullLogger"/> instance, which does not do
        ///  any logging. You can change this behaviour by assigning, for example, a
        ///  <see cref="SqlServerLogger"/> instance to do the heavy lifting.
        /// </remarks>

        public ILogger Logger { get; set; }


        /* ====== Constructors ====== */

        /// <summary>
        ///  Creates a new instance of the <see cref="BBEngine"/> class,
        ///  with a default set of rules.
        /// </summary>

        public BBEngine()
        {
            this.Settings = new Settings();
            this.Logger = GetDefaultLogger();
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

        /// <summary>
        ///  Creates a new instance of the <see cref="BBEngine"/> class,
        ///  with a custom settings object and a custom list of rules.
        /// </summary>
        /// <param name="rules">
        ///  A list of <see cref="IRule"/> objects, used to vet the web
        ///  requests.
        /// </param>

        public BBEngine(params IRule[] rules)
        {
            this.Settings = new Settings();
            this.Logger = GetDefaultLogger();
            this.Rules = rules.ToList();
        }


        /* ====== GetDefaultLogger ====== */

        /// <summary>
        ///  Creates a default logger.
        /// </summary>
        /// <remarks>
        ///  The default logger will be a SQL Server logger if the Bad Behavior
        ///  connection string is defined, otherwise it will be null.
        /// </remarks>

        public virtual ILogger GetDefaultLogger()
        {
            var cs = ConfigurationManager.ConnectionStrings["BadBehavior"];
            if (cs != null && cs.ProviderName == "System.Data.SqlClient")
                return new SqlServerLogger(cs.ConnectionString);
            else
                return new NullLogger();
        }


        /* ====== Methods ====== */

        /// <summary>
        ///  Validates a request.
        /// </summary>
        /// <param name="request">
        ///  The request to validate.
        /// </param>
        /// <exception cref="BadBehaviorException">
        ///  Indicates that the request failed to validate.
        /// </exception>
        /// <remarks>
        ///  This method will not throw exceptions (other than BadBehaviorException)
        ///  in a production environment. In this case, exceptions will be logged to
        ///  System.Diagnostics.Trace.
        /// </remarks>

        public virtual void ValidateRequest(HttpRequestBase request)
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

        /// <summary>
        ///  Handles an error and sends an appropriate response to the client.
        /// </summary>
        /// <param name="context">
        ///  The <see cref="HttpApplication"/> instance containing the web request.
        /// </param>
        /// <param name="ex">
        ///  The exception which was raised by the 
        /// </param>

        public virtual void HandleError(HttpApplication context, BadBehaviorException ex)
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

        /// <summary>
        ///  Returns the HTTP response content appropriate to the given error.
        /// </summary>
        /// <param name="ex">
        ///  The exceptoin which was raised.
        /// </param>
        /// <returns>
        ///  The HTML response to return to the client.
        /// </returns>

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

        /// <summary>
        ///  Creates a support key for a particular error condition.
        /// </summary>
        /// <param name="ipAddress">
        ///  The client's IP address.
        /// </param>
        /// <param name="errorCode">
        ///  The Bad Behavior support code for the condition which was raised.
        /// </param>
        /// <returns>
        ///  A formatted support key, based on the user's IP address followed by the support code.
        /// </returns>
        /// <remarks>
        ///  The support key will be in the format used by the original Bad Behavior: four sets of
        ///  four-digit hexadecimal numbers, separated by hyphens. The last two sets represent the
        ///  error code; this is preceded by octets representing the user's IP address.
        /// </remarks>

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

        /// <summary>
        ///  Called by the validators to raise an error.
        /// </summary>
        /// <param name="package">
        ///  The <see cref="Package"/> instance containing details of the request.
        /// </param>
        /// <param name="error">
        ///  The error condition detailing the problem.
        /// </param>

        public void Raise(Package package, Error error)
        {
            Raise(package, error, false);
        }

        /// <summary>
        ///  Called when an error or suspicious condition has been raised.
        /// </summary>
        /// <param name="package">
        ///  The <see cref="Package"/> instance containing details of the request.
        /// </param>
        /// <param name="error">
        ///  The error condition detailing the problem.
        /// </param>
        /// <param name="strict">
        ///  true if this is a strict condition (i.e. should only be trapped when
        ///  running in strict mode); otherwise false.
        /// </param>

        protected virtual void Raise(Package package, Error error, bool strict)
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

        /// <summary>
        ///  Called by the validators when a suspicious condition occurs.
        /// </summary>
        /// <param name="package">
        ///  The <see cref="Package"/> instance containing details of the request.
        /// </param>
        /// <param name="error">
        ///  The error condition detailing the problem.
        /// </param>
        /// <remarks>
        ///  Suspicious conditions (or strict-mode conditions) are only trapped when
        ///  running in strict mode, as while they often indicate bad behaviour, they
        ///  can also be triggered by more innocent conditions, such as malfunctioning
        ///  corporate proxy servers.
        /// </remarks>

        public void RaiseStrict(Package package, Error error)
        {
            Raise(package, error, true);
        }

        /// <summary>
        ///  Raised when an event has been trapped by Bad Behavior.
        /// </summary>

        public event BadBehaviorEventHandler BadBehavior;

        /// <summary>
        ///  Called when an event has been trapped by Bad Behavior. Can be overridden
        ///  to provide custom behaviour when an event has been logged.
        /// </summary>
        /// <param name="args">
        ///  A <see cref="BadBehaviorEventArgs"/> object containing information about
        ///  the bad request.
        /// </param>

        protected virtual void OnBadBehavior(BadBehaviorEventArgs args)
        {
            if (this.BadBehavior != null)
                this.BadBehavior(this, args);
        }

        private bool _configured = false;

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void Configure(IEnumerable<IConfigurator> configurators)
        {
            // This method may be called multiple times if IIS creates multiple HttpApplication
            // instances and hence multiple instances of the Bad Behavior HTTP module. Since
            // we're only using one BBEngine instance between all of them, we need to make sure
            // this method only gets called once.
            // See http://stackoverflow.com/questions/3370839

            if (_configured) return;
            foreach (var configurator in configurators)
                configurator.Configure(this);
            _configured = true;
        }
    }
}