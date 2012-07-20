using System;
using System.Collections.Generic;
using System.Reflection;

namespace BadBehavior.Rules
{
    public static class Errors
    {
        public static readonly Error BlacklistedIP = new Error(
            "136673cd", 403, Explanations.BlacklistedIP,
            "IP address found on external blacklist"
        );

        public static readonly Error AcceptMissing = new Error(
            "17566707", 403, Explanations.InvalidBrowserRequest,
            "Required header 'Accept' missing"
        );

        public static readonly Error Blacklist = new Error(
            "17f4e8c8", 403, Explanations.PermissionDenied,
            "User-Agent was found on blacklist"
        );

        public static readonly Error Httpbl = new Error(
            "2b021b1f", 403, Explanations.PossibleVirus,
            "IP address found on http:BL blacklist"
        );

        public static readonly Error InvalidMSIEWithTE = new Error(
            "2b90f772", 403, Explanations.InvalidMSIEWithTE,
            "Connection: TE present, not supported by MSIE"
        );

        public static readonly Error Http11Invalid = new Error(
            "41feed15", 400, Explanations.Http11Invalid,
            "Header 'Pragma' without 'Cache-Control' prohibited for HTTP/1.1 requests"
        );

        public static readonly Error CorruptReferer = new Error(
            "45b35e30", 400, Explanations.InvalidBrowserRequest,
            "Header 'Referer' is corrupt"
        );

        public static readonly Error TeWithoutConnectionTe = new Error(
            "582ec5e4", 400, Explanations.InvalidProxyServerRequest,
            "Header 'TE' present but TE not specified in 'Connection' header"
        );

        public static readonly Error BlankReferer = new Error(
            "69920ee5", 400, Explanations.InvalidBrowserRequest,
            "Header 'Referer' present but blank"
        );

        public static readonly Error InvalidCookies = new Error(
            "6c502ff1", 403, Explanations.PermissionDenied,
            "Bot not fully compliant with RFC 2965"
        );

        public static readonly Error FakeYahoobot = new Error(
            "71436a15", 403, Explanations.FakeSearchEngine,
            "User-Agent claimed to be Yahoo, claim appears to be false"
        );

        public static readonly Error RangeHeaderZero = new Error(
            "7ad04a8a", 400, Explanations.RangeHeaderZero,
            "Prohibited header \'Range\' present"
        );

        public static readonly Error InvalidRangeHeader = new Error(
            "7d12528e", 403, Explanations.PermissionDenied,
            "Prohibited header 'Range' or 'Content-Range' in POST request"
        );

        public static readonly Error BannedProxy = new Error(
            "939a6fbb", 403, Explanations.InvalidProxyServer,
            "Banned proxy server in use"
        );

        public static readonly Error InvalidVia = new Error(
            "9c9e4979", 403, Explanations.InvalidProxyServer,
            "Prohibited header 'via' present"
        );

        public static readonly Error Http10Expect = new Error(
            "a0105122", 417, Explanations.ExpectationFailed,
            "Header 'Expect' prohibited; resend without Expect"
        );

        public static readonly Error InvalidMSIEWindowsVersion = new Error(
            "a1084bad", 403, Explanations.PermissionDenied,
            "User-Agent claimed to be MSIE, with invalid Windows version"
        );

        public static readonly Error InvalidConnectionHeader = new Error(
            "a52f0448", 400, Explanations.MalfunctioningProxyServer,
            "Header 'Connection' contains invalid values"
        );

        public static readonly Error MaliciousConnectionHeader = new Error(
            "b0924802", 400, Explanations.PossibleMalware,
            "Incorrect form of HTTP/1.0 Keep-Alive"
        );

        public static readonly Error ProxyConnectionHeaderPresent = new Error(
            "b7830251", 400, Explanations.InvalidProxyRequest,
            "Prohibited header 'Proxy-Connection' present"
        );

        public static readonly Error ProhibitedHeader = new Error(
            "b9cc1d86", 403, Explanations.InvalidProxyServer,
            "Prohibited header 'X-Aaaaaaaaaa' or 'X-Aaaaaaaaaaaa' present"
        );

        public static readonly Error NotSameOrigin = new Error(
            "cd361abb", 403, Explanations.NotSameOrigin,
            "Referer did not point to a form on this site"
        );

        public static readonly Error TrackbackFromProxyServer = new Error(
            "d60b87c7", 403, Explanations.PossibleVirus,
            "Trackback received via proxy server"
        );

        public static readonly Error Malicious = new Error(
            "dfd9b1ad", 403, Explanations.PermissionDenied,
            "Request contained a malicious JavaScript or SQL injection attack."
        );

        public static readonly Error FakeWordPress = new Error(
            "e3990b47", 403, Explanations.PossibleVirus,
            "Obviously fake trackback received"
        );

        public static readonly Error FakeMSNbot = new Error(
            "e4de0453", 403, Explanations.FakeSearchEngine,
            "User-Agent claimed to be msnbot, claim appears to be false"
        );

        public static readonly Error TrackbackFromWebBrowser = new Error(
            "f0dcb3fd", 403, Explanations.PossibleVirus,
            "Web browser attempted to send a trackback"
        );

        public static readonly Error FakeGooglebot = new Error(
            "f1182195", 403, Explanations.FakeSearchEngine,
            "User-Agent claimed to be Googlebot, claim appears to be false."
        );

        public static readonly Error UserAgentMissing = new Error(
            "f9f2b8b9", 403, Explanations.UserAgentMissing,
            "A User-Agent is required but none was provided."
        );


        private static IDictionary<string, Error> table
            = new Dictionary<string, Error>(StringComparer.InvariantCultureIgnoreCase);

        static Errors()
        {
            foreach (var field in typeof(Errors).GetFields(BindingFlags.Static | BindingFlags.Public)) {
                Error err = field.GetValue(null) as Error;
                if (err != null) {
                    table.Add(err.Code, err);
                }
            }
        }

        public static Error Lookup(string code)
        {
            Error result;
            return table.TryGetValue(code, out result) ? result : null;
        }
    }
}
