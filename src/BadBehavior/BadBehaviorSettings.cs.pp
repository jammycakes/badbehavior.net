using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BadBehavior;

namespace $rootnamespace$
{
    /// <summary>
    ///  Configures Bad Behavior for use with this web application.
    /// </summary>

    public class BadBehaviorSettings : ISettings
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

        public IList<string> WhitelistIPRanges
        {
            get { return null; }
        }

        /// <summary>
        ///  User agent substrings to be whitelisted
        /// </summary>

        public IList<string> WhitelistUserAgents
        {
            get { return null; }
        }

        /// <summary>
        ///  URLs on our site to be whitelisted
        /// </summary>

        public IList<string> WhitelistUrls
        {
            get { return null; }
        }

        /// <summary>
        ///  Whether an httpbl key has been configured.
        /// </summary>

        public bool Httpbl
        {
            get { return false; }
        }

        /// <summary>
        ///  The httpbl key.
        /// </summary>

        public string HttpblKey
        {
            get { return null; }
        }

        /// <summary>
        ///  The httpbl threat level required to trap spam.
        /// </summary>

        public int HttpblThreatLevel
        {
            get { return 25; }
        }

        /// <summary>
        ///  The httpbl maximum age, in days.
        /// </summary>

        public int HttpblMaxAge
        {
            get { return 30; }
        }
    }
}