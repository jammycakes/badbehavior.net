using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BadBehavior.Configuration;

namespace BadBehavior
{
    /* ====== Settings class ====== */

    /// <summary>
    ///  Contains the Bad Behavior configuration settings.
    /// </summary>

    public class Settings
    {
        /// <summary>
        ///  Indicates that the Bad Behavior logs can be viewed on a remote server.
        ///  If this is not set, the logs can only be viewed by logging on to localhost.
        /// </summary>

        public bool AllowRemoteLogViewing { get; set; }

        /// <summary>
        ///  Indicates that Bad Behavior will run in Debug mode.
        ///  When this is set, non-Bad Behavior exceptions encountered when
        ///  validating or logging will be thrown, rather than just being
        ///  logged to System.Diagnostics.Trace.
        /// </summary>

        public bool Debug { get; set; }

        /// <summary>
        ///  Indicates that we allow postbacks from other sites.
        ///  Otherwise we'll enforce a same domain policy.
        /// </summary>

        public bool OffsiteForms { get; set; }

        /// <summary>
        ///  Indicates that we are operating in strict mode.
        /// </summary>

        public bool Strict { get; set; }

        /// <summary>
        ///  The e-mail address that should be contacted for support.
        /// </summary>

        public string SupportEmail { get; set; }

        /// <summary>
        ///  Indicates that this site is behind a reverse proxy.
        /// </summary>

        public virtual bool ReverseProxy { get; set; }

        /// <summary>
        ///  Gives a list of all the known reverse proxy addresses.
        ///  These can be specified as a list of CIDR netblocks.
        /// </summary>

        public string[] ReverseProxyAddresses { get; set; }

        /// <summary>
        ///  The HTTP header which gives the reverse proxy indirections.
        ///  Default: "X-Forwarded-For". If you use CloudFlare, this should be
        ///  set to "CF-Connecting-IP".
        /// </summary>

        public string ReverseProxyHeader { get; set; }

        /// <summary>
        ///  The IP address ranges, in CIDR format, to be whitelisted
        /// </summary>

        public string[] WhitelistIPRanges { get; set; }

        /// <summary>
        ///  User agent substrings to be whitelisted
        /// </summary>

        public string[] WhitelistUserAgents { get; set; }

        /// <summary>
        ///  URLs on our site to be whitelisted
        /// </summary>

        public string[] WhitelistUrls { get; set; }

        /// <summary>
        ///  Whether an httpbl key has been configured.
        /// </summary>

        public bool Httpbl { get; set; }

        /// <summary>
        ///  The httpbl key.
        /// </summary>

        public string HttpblKey { get; set; }

        /// <summary>
        ///  The httpbl threat level required to trap spam.
        /// </summary>

        public int HttpblThreatLevel { get; set; }

        /// <summary>
        ///  The httpbl maximum age, in days.
        /// </summary>

        public int HttpblMaxAge { get; set; }


        /* ====== Constructor ====== */

        /// <summary>
        ///  Creates a new instance of the <see cref="Settings"/> class
        ///  with some default values.
        /// </summary>

        public Settings()
        {
            var config = BadBehaviorConfigurationSection.Get();

            this.AllowRemoteLogViewing = config.AllowRemoteLogViewing;
            this.Debug = config.Debug;
            this.Httpbl = config.Httpbl != null;
            if (this.Httpbl) {
                this.HttpblKey = config.Httpbl.Key;
                this.HttpblMaxAge = config.Httpbl.MaxAge;
                this.HttpblThreatLevel = config.Httpbl.ThreatLevel;
            }
            this.OffsiteForms = config.OffsiteForms;
            this.ReverseProxy = config.ReverseProxy;
            this.ReverseProxyAddresses = config.ReverseProxyAddresses
                .Cast<ValueElement>().Select(x => x.Value).ToArray();
            this.ReverseProxyHeader = config.ReverseProxyHeader;
            this.Strict = config.Strict;
            this.SupportEmail = config.SupportEmail;
            if (config.WhiteList != null) {
                this.WhitelistIPRanges = config.WhiteList.IPRanges
                    .Cast<ValueElement>().Select(x => x.Value).ToArray();
                this.WhitelistUrls = config.WhiteList.Urls
                    .Cast<ValueElement>().Select(x => x.Value).ToArray();
                this.WhitelistUserAgents = config.WhiteList.UserAgents
                    .Cast<ValueElement>().Select(x => x.Value).ToArray();
            }
            else {
                this.WhitelistIPRanges = this.WhitelistUrls = this.WhitelistUserAgents
                    = new string[0];
            }
        }
    }
}
