using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BadBehavior;

namespace $rootnamespace$
{
    /* ====== BadBehaviorSettings ====== */

    /// <summary>
    ///  Configures Bad Behavior for use with this web application.
    /// </summary>

    public class BadBehaviorSettings : SettingsBase
    {
        /// <summary>
        ///  Indicates that the Bad Behavior logs can be viewed on a remote server.
        ///  If this is not set, the logs can only be viewed by logging on to localhost.
        /// </summary>

        public bool AllowRemoteLogViewing
        {
            get { return false; }
        }

        /// <summary>
        ///  Indicates that Bad Behavior will run in Debug mode.
        ///  When this is set, non-Bad Behavior exceptions encountered when
        ///  validating or logging will be thrown, rather than just being
        ///  logged to System.Diagnostics.Trace.
        /// </summary>

        public bool Debug
        {
            get { return false; }
        }

        /// <summary>
        ///  Indicates that we allow postbacks from other sites.
        ///  Otherwise we'll enforce a same domain policy.
        /// </summary>

        public bool OffsiteForms
        {
            get { return false; }
        }

        /// <summary>
        ///  Indicates that we are operating in strict mode.
        /// </summary>

        public bool Strict
        {
            get { return false; }
        }

        /// <summary>
        ///  The e-mail address that should be contacted for support.
        /// </summary>

        public string SupportEmail
        {
            get { return "bad.behavior@example.com"; }
        }

        /// <summary>
        ///  Indicates that this site is behind a reverse proxy.
        /// </summary>

        public bool ReverseProxy
        {
            get { return false; }
        }

        /// <summary>
        ///  Gives a list of all the known reverse proxy addresses.
        ///  These can be specified as a list of CIDR netblocks.
        /// </summary>

        public IList<string> ReverseProxyAddresses
        {
            get { return null; }
        }

        /// <summary>
        ///  The HTTP header which gives the reverse proxy indirections.
        ///  Default: "X-Forwarded-For". If you use CloudFlare, this should be
        ///  set to "CF-Connecting-IP".
        /// </summary>

        public string ReverseProxyHeader
        {
            get { return null; }
        }

        /// <summary>
        ///  The IP address ranges, in CIDR format, to be whitelisted
        /// </summary>
        /// <remarks>
        ///  Inappropriate whitelisting WILL expose you to spam, or cause Bad Behavior
        ///  to stop functioning entirely! DO NOT WHITELIST unless you are 100% CERTAIN
        ///  that you should.
        /// </remarks>

        public IList<string> WhitelistIPRanges
        {
            get
            { 
                return new string[] {
                    /* Digg */
                    // "64.191.203.0/24",
                    // "208.67.217.130",

                    /* RFC 1918 addresses */
                    // "10.0.0.0/8",
                    // "172.16.0.0/12",
                    // "192.168.0.0/16"
                }.ToList();
            }
        }

        /// <summary>
        ///  User agent strings to be whitelisted.
        /// </summary>
        /// <remarks>
        ///  User agents are matched by exact match only.
        /// </remarks>

        public IList<string> WhitelistUserAgents
        {
            get {
                return new string[] {
                    // "Mozilla/4.0 (It's me, let me in)"
                }.ToList();
            }
        }

        /// <summary>
        ///  URLs are matched from the first / after the server name up to, but not
        ///  including, the ? (if any). The URL to be whitelisted is a URL on YOUR site.
        ///  A partial URL match is permitted, so URL whitelist entries should be as
        ///  specific as possible, but no more specific than necessary. For instance,
        ///  "/example" would match "/example.aspx" and "/example/address".
        /// </summary>

        public IList<string> WhitelistUrls
        {
            get {
                return new string[] {
                    // "/example.aspx",
                    // "/openid/server"
                }.ToList();
            }
        }

        /// <summary>
        ///  Whether a key for the Project Honeypot HTTP:BL server has been configured.
        /// </summary>

        public bool Httpbl
        {
            get { return false; }
        }

        /// <summary>
        ///  The key for the Project Honeypot HTTP:BL server.
        /// </summary>

        public string HttpblKey
        {
            get { return null; }
        }

        /// <summary>
        ///  The Project Honeypot HTTP:BL threat level.
        /// </summary>

        public int HttpblThreatLevel
        {
            get { return 25; }
        }

        /// <summary>
        ///  The Project Honeypot HTTP:BL maximum age, in days.
        /// </summary>

        public int HttpblMaxAge
        {
            get { return 30; }
        }
    }
}