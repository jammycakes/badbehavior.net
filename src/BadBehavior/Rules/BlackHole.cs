using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static readonly Dictionary<string, IPAddress[]> blackholeExceptions
            = new Dictionary<string, IPAddress[]>() {
            { "sbl-xbl.spamhaus.org", new IPAddress[] {
                IPAddress.Parse("127.0.0.4") } },    // CBL is problematic
            { "dnsbl.sorbs.net", new IPAddress[] {
                IPAddress.Parse("127.0.0.10") } }         // Dynamic IPs only
        };

        public string GetDnsblLookup(IPAddress ip, string dnsbl)
        {
            var bytes = ip.GetAddressBytes().Reverse().ToArray();
            string s = new IPAddress(bytes).ToString();
            return s + "." + dnsbl;
        }

        public RuleProcessing Validate(Package package)
        {
            if (package.OriginatingIP.AddressFamily == AddressFamily.InterNetwork) {
                ValidateDnsBlackHole(package);
                if (package.Settings.Httpbl)
                    if (ValidateHttpbl(package)) return RuleProcessing.Stop; // whitelisted
            }

            return RuleProcessing.Continue;
        }

        private void ValidateDnsBlackHole(Package package)
        {
            foreach (string dnsbl in blackholeLists) {
                var find = GetDnsblLookup(package.OriginatingIP, dnsbl);
                IPHostEntry dns;
                try {
                    dns = Dns.GetHostEntry(find);
                }
                catch (SocketException ex) {
                    /*
                     * This error means one of two things:
                     * (a) we haven't been able to query the DNS server for some reason.
                     * (b) the DNS server has no data.
                     * In both cases, keep calm and carry on.
                     */
                    if (ex.SocketErrorCode != SocketError.NoData)
                        Trace.TraceWarning("An error was encountered when attempting to connect to "
                            + dnsbl + ". Exception details: " + ex.ToString());
                    return;
                }
                if (dns != null && dns.AddressList != null) {
                    IPAddress[] exceptions;
                    if (blackholeExceptions.TryGetValue(dnsbl, out exceptions))
                        AssertNotListed(package, dns.AddressList.Except(exceptions));
                    else
                        AssertNotListed(package, dns.AddressList);
                }
            }
        }

        private void AssertNotListed(Package package, IEnumerable<IPAddress> addresses)
        {
            if (addresses.Any())
                package.Raise(Errors.BlacklistedIP);
        }

        private bool ValidateHttpbl(Package package)
        {
            var test = package.Settings.HttpblKey + "." +
                GetDnsblLookup(package.OriginatingIP, "dnsbl.httpbl.org");
            IPHostEntry dns;
            try {
                dns = Dns.GetHostEntry(test);
            }
            catch (SocketException ex) {
                // As above. Assume not a search engine, so don't whitelist.
                if (ex.SocketErrorCode != SocketError.NoData)
                    Trace.TraceWarning("An error was encountered when attempting to connect to dnsbl.httbl.org. "
                        + "Exception details: " + ex.ToString());
                return false;
            }
            if (dns.AddressList.Any()) {
                var ip = dns.AddressList[0].GetAddressBytes();
                if (ip[0] == 127
                    && (ip[3] & 7) != 0
                    && (int)ip[2] >= package.Settings.HttpblThreatLevel
                    && (int)ip[1] <= package.Settings.HttpblMaxAge)
                    package.Raise(Errors.Httpbl);
                else return ip[3] == 0; // Check if search engine.
            }

            return false;
        }
    }
}
