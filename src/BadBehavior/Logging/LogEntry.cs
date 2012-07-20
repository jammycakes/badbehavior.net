using System;
using System.Net;
using System.Text;

namespace BadBehavior.Logging
{
    /* ====== LogEntry class ====== */

    /// <summary>
    ///  Contains an entry in the Bad Behavior logs.
    /// </summary>

    public class LogEntry
    {
        /// <summary>
        ///  The originating IP address of the violating request.
        /// </summary>

        public IPAddress IP { get; set; }

        /// <summary>
        ///  The date and time of the violation.
        /// </summary>

        public DateTime Date { get; set; }

        /// <summary>
        ///  The HTTP request method (GET or POST etc).
        /// </summary>

        public string RequestMethod { get; set; }

        /// <summary>
        ///  The URI on our site that was requested.
        /// </summary>

        public string RequestUri { get; set; }

        /// <summary>
        ///  The HTTP protocol version (HTTP/1.0 or HTTP/1.1)
        /// </summary>

        public string ServerProtocol { get; set; }

        /// <summary>
        ///  A string containing the list of HTTP headers, one on
        ///  each line, in the format key: value
        /// </summary>

        public string HttpHeaders { get; set; }

        /// <summary>
        ///  The user agent string.
        /// </summary>

        public string UserAgent { get; set; }

        /// <summary>
        ///  The HTTP POST body, one entry on each line, in the format
        ///  key: value.
        /// </summary>

        public string RequestEntity { get; set; }

        /// <summary>
        ///  The support key for the violation.
        /// </summary>
        /// <remarks>
        ///  As per Bad Behavior original, 00000000 will be used for
        ///  permitted strict-mode conditions.
        /// </remarks>

        public string Key { get; set; }

        /// <summary>
        ///  The reverse DNS lookup for the IP address.
        /// </summary>

        public string ReverseDns { get; set; }

        /* ====== Constructors ====== */

        /// <summary>
        ///  Creates a new instance of the <see cref="LogEntry"/> class.
        /// </summary>

        public LogEntry()
        { }

        /// <summary>
        ///  Creates a new instance of the <see cref="LogEntry"/> class
        ///  from an exception condition.
        /// </summary>
        /// <param name="ex">
        ///  The <see cref="BadBehaviorException"/> representing this violation.
        /// </param>
        /// <param name="thrown">
        ///  true if the exception will be thrown after logging is carried out;
        ///  false if it is a strict-mode violation and Bad Behavior is not running
        ///  in strict mode.
        /// </param>

        public LogEntry(BadBehaviorException ex, bool thrown)
        {
            this.IP = ex.Package.OriginatingIP;
            this.Date = DateTime.UtcNow;
            this.RequestMethod = ex.Package.Request.HttpMethod;
            this.RequestUri = ex.Package.Request.RawUrl;
            if (ex.Package.Request.ServerVariables != null)
                this.ServerProtocol = ex.Package.Request.ServerVariables["SERVER_PROTOCOL"];
            this.HttpHeaders = PackageDict(ex.Package.Request.Headers);
            this.UserAgent = ex.Package.Request.UserAgent;
            this.RequestEntity = PackageDict(ex.Package.Request.Form);
            this.Key = thrown ? ex.Error.Code : "00000000";
        }

        private string PackageDict(System.Collections.Specialized.NameValueCollection dict)
        {
            if (dict == null) return null;

            var sb = new StringBuilder();
            foreach (string str in dict.Keys) {
                sb.AppendLine(str + ": " + dict[str]);
            }
            return sb.ToString();
        }
    }
}
