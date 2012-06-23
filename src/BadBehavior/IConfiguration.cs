using System;

namespace BadBehavior
{
    /// <summary>
    ///  Provides an interface to the configuration settings.
    /// </summary>

    public interface IConfiguration
    {
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

        System.Collections.Generic.IList<string> ReverseProxyAddresses { get; }

        /// <summary>
        ///  The HTTP header which gives the reverse proxy indirections.
        ///  Default: "X-Forwarded-For". If you use CloudFare, this should be
        ///  set to "CF-Connecting-IP".
        /// </summary>

        string ReverseProxyHeader { get; }
    }
}
