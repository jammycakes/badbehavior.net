using System;
using System.Collections.Generic;
using System.Linq;

namespace BadBehavior
{
    /// <summary>
    ///  Provides an interface to the configuration settings.
    /// </summary>

    public abstract class SettingsBase
    {
        /// <summary>
        ///  Indicates that the Bad Behavior logs can be viewed on a remote server.
        ///  If this is not set, the logs can only be viewed by logging on to localhost.
        /// </summary>

        public virtual bool AllowRemoteLogViewing { get { return false; } }

        /// <summary>
        ///  Indicates that Bad Behavior will run in Debug mode.
        ///  When this is set, non-Bad Behavior exceptions encountered when
        ///  validating or logging will be thrown, rather than just being
        ///  logged to System.Diagnostics.Trace.
        /// </summary>

        public virtual bool Debug { get { return false; } }

        /// <summary>
        ///  Indicates that we allow postbacks from other sites.
        ///  Otherwise we'll enforce a same domain policy.
        /// </summary>

        public virtual bool OffsiteForms { get { return false; } }

        /// <summary>
        ///  Indicates that we are operating in strict mode.
        /// </summary>

        public virtual bool Strict { get { return false; } }

        /// <summary>
        ///  The e-mail address that should be contacted for support.
        /// </summary>

        public virtual string SupportEmail { get { return String.Empty; } }

        /// <summary>
        ///  Indicates that this site is behind a reverse proxy.
        /// </summary>

        public virtual bool ReverseProxy { get { return false; } }

        /// <summary>
        ///  Gives a list of all the known reverse proxy addresses.
        ///  These can be specified as a list of CIDR netblocks.
        /// </summary>

        public virtual IList<string> ReverseProxyAddresses
        {
            get
            {
                return new string[0].ToList();
            }
        }

        /// <summary>
        ///  The HTTP header which gives the reverse proxy indirections.
        ///  Default: "X-Forwarded-For". If you use CloudFlare, this should be
        ///  set to "CF-Connecting-IP".
        /// </summary>

        public virtual string ReverseProxyHeader { get { return "X-Forwarded-For"; } }

        /// <summary>
        ///  The IP address ranges, in CIDR format, to be whitelisted
        /// </summary>

        public virtual IList<string> WhitelistIPRanges
        {
            get
            {
                return new string[0].ToList();
            }
        }

        /// <summary>
        ///  User agent substrings to be whitelisted
        /// </summary>

        public virtual IList<string> WhitelistUserAgents
        {
            get
            {
                return new string[0].ToList();
            }
        }

        /// <summary>
        ///  URLs on our site to be whitelisted
        /// </summary>

        public virtual IList<string> WhitelistUrls
        {
            get
            {
                return new string[0].ToList();
            }
        }

        /// <summary>
        ///  Whether an httpbl key has been configured.
        /// </summary>

        public virtual bool Httpbl { get { return false; } }

        /// <summary>
        ///  The httpbl key.
        /// </summary>

        public virtual string HttpblKey { get { return null; } }

        /// <summary>
        ///  The httpbl threat level required to trap spam.
        /// </summary>

        public virtual int HttpblThreatLevel { get { return 25; } }

        /// <summary>
        ///  The httpbl maximum age, in days.
        /// </summary>

        public virtual int HttpblMaxAge { get { return 30; } }
    }
}
