using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BadBehavior.Rules
{
    public class BlackHole : IRule
    {
        // Only conservative lists
        private static readonly string[] blackholeLists = new string[] {
            "sbl-xbl.spamhaus.org", // All around nasties
            // "dnsbl.sorbs.net",      // Old useless data.
            // "list.dsbl.org",        // Old useless data.
            // "dnsbl.ioerror.us",     // Bad Behavior Blackhole
        };

        private static readonly Dictionary<string, string> blackholeExceptions
            = new Dictionary<string, string>() {
            { "sbl-xbl.spamhaus.org", "127.0.0.4" },    // CBL is problematic
            { "dnsbl.sorbs.net", "127.0.0.10" }         // Dynamic IPs only
        };

        public string GetDnsblLookup(IPAddress ip, string dnsbl)
        {
            var bytes = ip.GetAddressBytes().Reverse().ToArray();
            string s = new IPAddress(bytes).ToString();
            return s + "." + dnsbl;
        }

        public RuleProcessing Validate(Package package)
        {
            if (package.OriginatingIP.AddressFamily != AddressFamily.InterNetwork)
                return RuleProcessing.Continue;



            return RuleProcessing.Continue;
        }
    }
}
