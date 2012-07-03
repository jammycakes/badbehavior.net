using System;
using System.Collections.Generic;

namespace BadBehavior
{
    /// <summary>
    ///  Provides an interface to the configuration settings.
    /// </summary>

    public interface ISettings
    {
        /// <summary>
        ///  Indicates that we allow postbacks from other sites.
        ///  Otherwise we'll enforce a same domain policy.
        /// </summary>

        bool OffsiteForms { get; }

        /// <summary>
        ///  Indicates that we are operating in strict mode.
        /// </summary>

        bool Strict { get; }

        /// <summary>
        ///  The e-mail address that should be contacted for support.
        /// </summary>

        string SupportEmail { get; }

        /// <summary>
        ///  Indicates that this site is behind a reverse proxy.
        /// </summary>

        bool ReverseProxy { get; }

        /// <summary>
        ///  Gives a list of all the known reverse proxy addresses.
        ///  These can be specified as a list of CIDR netblocks.
        /// </summary>

        IList<string> ReverseProxyAddresses { get; }

        /// <summary>
        ///  The HTTP header which gives the reverse proxy indirections.
        ///  Default: "X-Forwarded-For". If you use CloudFlare, this should be
        ///  set to "CF-Connecting-IP".
        /// </summary>

        string ReverseProxyHeader { get; }

        /// <summary>
        ///  The IP address ranges, in CIDR format, to be whitelisted
        /// </summary>

        IList<string> WhitelistIPRanges { get; }

        /// <summary>
        ///  User agent substrings to be whitelisted
        /// </summary>

        IList<string> WhitelistUserAgents { get; }

        /// <summary>
        ///  URLs on our site to be whitelisted
        /// </summary>

        IList<string> WhitelistUrls { get; }

        /// <summary>
        ///  Whether an httpbl key has been configured.
        /// </summary>

        bool Httpbl { get; }

        /// <summary>
        ///  The httpbl key.
        /// </summary>

        string HttpblKey { get; }

        /// <summary>
        ///  The httpbl threat level required to trap spam.
        /// </summary>

        int HttpblThreatLevel { get; }

        /// <summary>
        ///  The httpbl maximum age, in days.
        /// </summary>

        int HttpblMaxAge { get; }
    }
}
