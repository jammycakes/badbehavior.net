using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BadBehavior
{
    public class Package
    {
        public BBEngine Engine { get; private set; }

        /// <summary>
        ///  The configuration settings to be used with this package.
        /// </summary>

        public IConfiguration Configuration { get { return Engine.Configuration; } }

        /// <summary>
        ///  Gets the HTTP headers case insensitively by key.
        /// </summary>

        public IDictionary<string, string> Headers { get; private set; }

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

        public bool IsBrowser { get; set; }



        /* ====== Constructor ====== */

        public Package(HttpRequestBase request, BBEngine engine)
        {
            this.Engine = engine;
            this.Request = request;
            this.Headers = new Dictionary<string, string>
                (this.Request.Headers.Count, StringComparer.InvariantCultureIgnoreCase);
            foreach (string key in request.Headers.Keys) {
                this.Headers[key] = request.Headers[key];
            }
            this.UserAgentI = this.Request.UserAgent == null
                ? null : this.Request.UserAgent.ToLowerInvariant();
            this.OriginatingIP = FindOriginatingIP();

            this.IsBrowser = false;
        }

        // If this is reverse-proxied or load balanced, obtain the actual client IP
        // See bb2_reverse_proxy function in core.inc.php

        private IPAddress FindOriginatingIP()
        {
            if (Configuration.ReverseProxy
                && this.Headers.ContainsKey(Configuration.ReverseProxyHeader)) {
                string[] addresses = Regex.Split
                    (this.Headers[Configuration.ReverseProxyHeader], @"[\s,]+");
                // Skip our known proxies and private addresses.
                string[] knownProxies = Configuration.ReverseProxyAddresses.ToArray();
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

        public void Throw(IRule validation, Error error)
        {
            this.Engine.Throw(validation, this, error);
        }
    }
}
