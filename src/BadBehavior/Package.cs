using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using BadBehavior.Util;

namespace BadBehavior
{
    /* ====== Package class ====== */

    /// <summary>
    ///  Contains a Bad Behavior-packaged version of the HTTP request.
    /// </summary>
    /// <remarks>
    ///  This class is a strongly-typed version of the $package variable
    ///  that you will see scattered around the original Bad Behavior
    ///  source code.
    /// </remarks>

    public class Package
    {
        /// <summary>
        ///  The <see cref="BBEngine"/> instance that is validating this package.
        /// </summary>

        public BBEngine Engine { get; private set; }

        /// <summary>
        ///  The configuration settings to be used with this package.
        /// </summary>

        public SettingsBase Settings { get { return Engine.Settings; } }

        /// <summary>
        ///  Gets the HTTP headers case insensitively by key.
        /// </summary>

        public NameValueCollection HeadersMixed { get; private set; }

        /// <summary>
        ///  Gets the <see cref="HttpRequestBase"/> instance being validated.
        /// </summary>

        public HttpRequestBase Request { get; private set; }

        /// <summary>
        ///  Gets the user agent string, translated into lower case.
        /// </summary>

        public string UserAgentI { get; private set; }

        /// <summary>
        ///  The originating IP address, allowing for reverse proxies if set.
        /// </summary>

        public IPAddress OriginatingIP { get; private set; }


        /* ====== Properties computed by the validators ====== */

        /// <summary>
        ///  Gets or sets a value indicating whether this request is coming from
        ///  something that claims to be a web browser.
        /// </summary>

        public bool IsBrowser { get; set; }


        /* ====== Constructor ====== */

        /// <summary>
        ///  Creates a new instance of the <see cref="Package"/> class.
        /// </summary>
        /// <param name="request">
        ///  The HTTP request being validated.
        /// </param>
        /// <param name="engine">
        ///  The <see cref="BBEngine"/> instance performing the validation.
        /// </param>

        public Package(HttpRequestBase request, BBEngine engine)
        {
            this.Engine = engine;
            this.Request = request;
            this.HeadersMixed = new NameValueCollection
                (this.Request.Headers.Count, HeadersMixedComparer.Instance);
            this.HeadersMixed.Add(request.Headers);
            this.UserAgentI = this.Request.UserAgent == null
                ? null : this.Request.UserAgent.ToLowerInvariant();
            this.OriginatingIP = FindOriginatingIP();

            this.IsBrowser = false;
        }

        // If this is reverse-proxied or load balanced, obtain the actual client IP
        // See bb2_reverse_proxy function in core.inc.php

        private IPAddress FindOriginatingIP()
        {
            if (Settings.ReverseProxy
                && this.HeadersMixed.ContainsKey(Settings.ReverseProxyHeader)) {
                string[] addresses = Regex.Split
                    (this.HeadersMixed[Settings.ReverseProxyHeader], @"[\s,]+");
                // Skip our known proxies and private addresses.
                string[] knownProxies = Settings.ReverseProxyAddresses.ToArray();
                foreach (string address in addresses) {
                    IPAddress ip = Functions.SafeParseIP(address);
                    if (ip != null && !ip.MatchCidr(knownProxies) && !ip.IsRfc1918()) {
                        return ip;
                    }
                }
                // If we get here, someone is playing a trick on us.
                // (Specifically: no reverse proxies are reporting the *real* original IP address)
                // In this case, BB original just uses the remote address. So shall we.
            }
            return Functions.SafeParseIP(Request.UserHostAddress);
        }

        /// <summary>
        ///  Called when a request has been rejected by a rule.
        /// </summary>
        /// <param name="error">
        ///  The <see cref="Error"/> condition giving details of the violation.
        /// </param>

        public void Raise(Error error)
        {
            this.Engine.Raise(this, error);
        }

        /// <summary>
        ///  Called when a request has been flagged as suspicious by a rule.
        ///  The request will be rejected if and only if Bad Behavior is running
        ///  in strict mode.
        /// </summary>
        /// <param name="error">
        ///  The <see cref="Error"/> condition giving details of the violation.
        /// </param>

        public void RaiseStrict(Error error)
        {
            this.Engine.RaiseStrict(this, error);
        }
    }
}
