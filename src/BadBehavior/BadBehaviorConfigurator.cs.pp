using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BadBehavior;

namespace $rootnamespace$
{
    public class BadBehaviorConfigurator : IConfigurator
    {
        public void Configure(BBEngine engine)
        {
            var settings = engine.Settings;

            /*
             * If you have any configuration settings that you wish to apply
             * to Bad Behavior, you can do so here.
             */

            /*
             * Whitelisting examples
             * Inappropriate whitelisting WILL expose you to spam, or cause Bad Behavior
             * to stop functioning entirely! DO NOT WHITELIST unless you are 100% CERTAIN
             * that you should.
             * 
             * IP address ranges use the CIDR format.
             */

            //settings.WhitelistIPRanges = new string[] {
            //    // Digg
            //    "64.191.203.0/24",
            //    "208.67.217.130",
            //    // RFC 1918 addresses
            //    "10.0.0.0/8",
            //    "172.16.0.0/12",
            //    "192.168.0.0/16"
            //};
            
            /*
             * User agents are matched by exact match only.
             */

            //settings.WhitelistUserAgents = new string[] {
            //    "Mozilla/4.0 (It's me, let me in)"
            //};

            /*
             * URLs are matched from the first / after the server name up to, but not
             * including, the ? (if any). The URL to be whitelisted is a URL on YOUR site.
             * A partial URL match is permitted, so URL whitelist entries should be as
             * specific as possible, but no more specific than necessary. For instance,
             * "/example" would match "/example.php" and "/example/address".
             */

            //settings.WhitelistUrls = new string[] {
            //    // Example: Whitelist static contents in an ASP.NET MVC file
            //    "/Scripts/",
            //    "/Content/"
            //};

            /*
             * If you want to set up a custom logger, you can do so like this.
             */

            //engine.Logger = new BadBehavior.Logging.SqlServer.SqlServerLogger
            //    (ConfigurationManager.ConnectionStrings["BadBehavior"].ConnectionString);
        }
    }
}
