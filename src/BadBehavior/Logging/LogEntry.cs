using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BadBehavior.Logging
{
    public class LogEntry
    {
        public IPAddress IP { get; set; }

        public DateTime Date { get; set; }

        public string RequestMethod { get; set; }

        public string RequestUri { get; set; }

        public string ServerProtocol { get; set; }

        public string HttpHeaders { get; set; }

        public string UserAgent { get; set; }

        public string RequestEntity { get; set; }

        public string Key { get; set; }

        public LogEntry()
        { }

        public LogEntry(BadBehaviorException ex, bool thrown)
        {
            this.IP = ex.Package.OriginatingIP;
            this.Date = DateTime.UtcNow;
            this.RequestMethod = ex.Package.Request.HttpMethod;
            this.RequestUri = ex.Package.Request.RawUrl;
            if (ex.Package.Request.ServerVariables != null)
                this.ServerProtocol = ex.Package.Request.ServerVariables["HTTP_PROTOCOL"];
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
