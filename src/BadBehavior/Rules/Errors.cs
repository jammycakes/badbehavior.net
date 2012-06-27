using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Rules
{
    internal static class Errors
    {
        public static readonly Error EAcceptMissing = new Error(
            "17566707", 403, Explanations.AcceptMissing,
            "Required header 'Accept' missing"
        );

        public static readonly Error EBlacklist = new Error(
            "17f4e8c8", 403, Explanations.Blacklisted,
            "User-Agent was found on blacklist"
        );

        public static readonly Error EInvalidMSIEWithTE = new Error(
            "2b90f772", 403, Explanations.InvalidMSIEWithTE,
            "Connection: TE present, not supported by MSIE"
        );

        public static readonly Error EHttp11Invalid = new Error(
            "41feed15", 400, Explanations.Http11Invalid,
            "Header 'Pragma' without 'Cache-Control' prohibited for HTTP/1.1 requests"
        );

        public static readonly Error EInvalidCookies = new Error(
            "6c502ff1", 403, Explanations.InvalidCookies,
            "Bot not fully compliant with RFC 2965"
        );

        public static readonly Error ERangeHeaderZero = new Error(
            "7ad04a8a", 400, Explanations.RangeHeaderZero,
            "Prohibited header \'Range\' present"
        );

        public static readonly Error EInvalidRangeHeader = new Error(
            "7d12528e", 403, Explanations.InvalidRangeHeader,
            "Prohibited header 'Range' or 'Content-Range' in POST request"
        );

        public static readonly Error EHttp10Expect = new Error(
            "a0105122", 417, Explanations.Http10Expect,
            "Header 'Expect' prohibited; resend without Expect"
        );

        public static readonly Error EInvalidMSIEWindowsVersion = new Error(
            "a1084bad", 403, Explanations.InvalidMSIEWindowsVersion,
            "User-Agent claimed to be MSIE, with invalid Windows version"
        );

        public static readonly Error EMalicious = new Error(
            "dfd9b1ad", 403, Explanations.Malicious,
            "Request contained a malicious JavaScript or SQL injection attack."
        );

        public static readonly Error EUserAgentMissing = new Error(
            "f9f2b8b9", 403, Explanations.UserAgentMissing,
            "A User-Agent is required but none was provided."
        );
    }
}
