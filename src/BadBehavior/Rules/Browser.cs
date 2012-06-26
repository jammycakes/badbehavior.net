using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadBehavior.Rules
{
    /* ====== Browser checks ====== */

    public class Browser : IRule
    {
        public RuleResult Validate(Package package)
        {
            // See core.inc.php in BB original.

            if (package.UserAgentI.Contains("; msie")) {
                package.IsBrowser = true;
                if (package.UserAgentI.Contains("opera"))
                    ValidateOpera(package);
                else
                    ValidateMSIE(package);
            }
            else if (package.UserAgentI.Contains("konqueror")) {
                package.IsBrowser = true;
                ValidateKonqueror(package);
            }
            else if (package.UserAgentI.Contains("opera")) {
                package.IsBrowser = true;
                ValidateOpera(package);
            }
            else if (package.UserAgentI.Contains("safari")) {
                package.IsBrowser = true;
                ValidateSafari(package);
            }
            else if (package.UserAgentI.Contains("lynx")) {
                package.IsBrowser = true;
                ValidateLynx(package);
            }
            else if (package.UserAgentI.Contains("movabletype")) {
                package.IsBrowser = false;
                ValidateMovableType(package);
            }
            else if (package.UserAgentI.StartsWith("mozilla")) {
                package.IsBrowser = true;
                ValidateMozilla(package);
            }

            return RuleResult.Continue;
        }

        /* ====== Error objects ====== */

        private static readonly Error EAcceptMissing = new Error
            ("17566707", 403, Explanations.AcceptMissing, "Required header 'Accept' missing");
        private static readonly Error EInvalidMSIEWindowsVersion = new Error
            ("a1084bad", 403, Explanations.InvalidMSIEWindowsVersion,
            "User-Agent claimed to be MSIE, with invalid Windows version");
        private static readonly Error EInvalidMSIEWithTE = new Error
            ("2b90f772", 403, Explanations.InvalidMSIEWithTE,
            "Connection: TE present, not supported by MSIE");
        private static readonly Error EInvalidRangeHeader = new Error
            ("7d12528e", 403, Explanations.InvalidRangeHeader,
            "Prohibited header 'Range' or 'Content-Range' in POST request");


        /* ====== Assertions ====== */

        // Assert that the "Accept" HTTP header is present. Bona fide browsers all send this.

        private void AssertAccept(Package package)
        {
            if (!package.Headers.ContainsKey("Accept"))
                package.Throw(this, EAcceptMissing);
        }

        /* ====== Browser checks ====== */

        // See browser.inc.php in original BB.

        // Validates user agents claiming to be Konqueror.

        private void ValidateKonqueror(Package package)
        {
            // CafeKelsa is a dev project at Yahoo which indexes job listings for
            // Yahoo! HotJobs. It identifies as Konqueror so we skip these checks.
            if (package.UserAgentI.Contains("yahooseeker/cafekelsa")) return;
            if (Functions.MatchCidr(package.OriginatingIP, "209.73.160.0/19")) return;

            AssertAccept(package);
        }

        // Validates user agents claiming to be Lynx

        private void ValidateLynx(Package package)
        {
            AssertAccept(package);
        }

        // Validates user agents claiming to be Mozilla

        private void ValidateMozilla(Package package)
        {
            // First off, workaround for Google Desktop, until they fix it FIXME
            // Google Desktop fixed it, but apparently some old versions are
            // still out there. :(
            // Always check accept header for Mozilla user agents

            if (package.Request.UserAgent.Contains("Google Desktop")) return;
            if (package.Request.UserAgent.Contains("PLAYSTATION 3")) return;
            AssertAccept(package);
        }

        // Validates user agents claiming to be MSIE

        private void ValidateMSIE(Package package)
        {
            AssertAccept(package);

            // MSIE does NOT send "Windows ME" or "Windows XP" in the user agent
            if (package.Request.UserAgent.Contains("Windows ME") ||
                package.Request.UserAgent.Contains("Windows XP") ||
                package.Request.UserAgent.Contains("Windows 2000") ||
                package.Request.UserAgent.Contains("Win32"))
                package.Throw(this, EInvalidMSIEWindowsVersion);

            // MSIE does NOT send Connection: TE but Akamai does
            // Bypass this test when Akamai detected
            if (package.Headers.ContainsKey("Akamai-Origin-Hop")) return;

            // The latest version of IE for Windows CE also uses Connection: TE
            if (package.Request.UserAgent.Contains("IEMobile")) return;

            if (package.Headers.ContainsKey("Connection")) {
                if (Regex.Match(package.Headers["Connection"], @"\bTE\b").Success) {
                    package.Throw(this, EInvalidMSIEWithTE);
                }
            }
        }

        // Validates user agents claiming to be Opera

        private void ValidateOpera(Package package)
        {
            AssertAccept(package);
        }

        // Validates user agents claiming to be Safari

        private void ValidateSafari(Package package)
        {
            AssertAccept(package);
        }


        /* ====== Movable Type ====== */

        // See movabletype.inc.php in BB original.

        private void ValidateMovableType(Package package)
        {
            // Is it a trackback?
            if (package.Request.HttpMethod == "POST") {
                if (package.Headers.ContainsKey("Range")
                    && package.Headers["Range"] == "bytes=0-99999") {
                        package.Throw(this, EInvalidRangeHeader);
                }
            }
        }
    }
}
