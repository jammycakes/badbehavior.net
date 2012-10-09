using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BadBehavior.Admin;
using BadBehavior.Logging;
using BadBehavior.Rules;
using BadBehavior.Util;
using System;
using System.Text.RegularExpressions;

namespace BadBehavior
{
    /* ====== BadBehaviorHandler class ====== */

    /// <summary>
    ///  An HTTP handler to display and manage the Bad Behavior event logs.
    /// </summary>
    /// <remarks>
    ///  Assuming you've used the web.config transforms in the NuGet package,
    ///  this handler will respond to requests for ~/BadBehavior.axd
    /// </remarks>

    public class BadBehaviorHandler : IHttpHandler
    {
        private LogQuery query;

        /// <summary>
        ///  Gets the <see cref="HttpContextBase"/> for this request
        ///  and its corresponding response..
        /// </summary>

        public HttpContextBase Context { get; set; }

        /// <summary>
        ///  Returns false to indicate that this handler is not reusable
        ///  (because it keeps its own copy of the HTTP context).
        /// </summary>

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        ///  Processes a web request to BadBehavior.axd.
        /// </summary>
        /// <param name="context">
        ///  The <see cref="HttpContext"/> for this request and its corresponding response.
        /// </param>

        public void ProcessRequest(HttpContext context)
        {
            this.Context = new HttpContextWrapper(context);
            if (!BBEngine.Instance.Settings.AllowRemoteLogViewing) {
                AssertLocal();
            }

            if (this.Context.Request.RequestType.ToUpper() == "POST"
                && this.Context.Request.Form.ContainsKey("clear")) {
                ClearLogs();
            }

            this.query = new LogQuery(Context.Request.QueryString);
            var content = this.GetContent();
            this.Context.Response.ContentType = "text/html";
            this.Context.Response.Write(content);
        }

        private void ClearLogs()
        {
            if (BBEngine.Instance.Logger != null)
                BBEngine.Instance.Logger.Clear();
        }

        private void AssertLocal()
        {
            if (!this.Context.Request.IsLocal)
                throw new HttpException
                    (404, "The resource you requested was not found on this server.");
        }

        const string templatePrefix = "BadBehavior.Admin.templates.";
        static readonly Template master = Template.FromResource(templatePrefix + "_master.html");

        private string GetView(string viewName, IDictionary<string, string> parameters)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            var view = Template.FromResource(templatePrefix + viewName + ".html");
            var content = view.Process(parameters);
            return master.Process(new Dictionary<string, string> {
                { "content", content },
                { "version", BBEngine.InformationalVersion }
            });
        }

        private string GetFilter(string filter, string value)
        {
            var baseQuery = this.query.Clone();
            baseQuery.Filter = filter;
            baseQuery.FilterValue = value;
            return new Uri(Context.Request.Url, "?" + baseQuery.ToString()).ToString();
        }

        private string BuildTableRows(IEnumerable<LogEntry> entries)
        {
            var tpl = Template.FromResource("BadBehavior.Admin.templates.row.html");
            var sb = new StringBuilder();
            bool alt = false;

            foreach (var entry in entries) {
                var error = Errors.Lookup(entry.Key);
                string row = tpl.Process(new Dictionary<string, string>() {
                    { "Date", entry.Date.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "IP", entry.IP.ToString() },
                    { "LogMessage", error.Log },
                    { "KeyFilter", GetFilter("Key", error.Code) },
                    { "IPFilter", GetFilter("IP", entry.IP.ToString()) },
                    { "RequestMethodFilter", GetFilter("RequestMethod", entry.RequestMethod) },
                    { "ProtocolFilter", GetFilter("ServerProtocol", entry.ServerProtocol) },
                    { "RequestMethod", entry.RequestMethod },
                    { "Url", entry.RequestUri },
                    { "Protocol", entry.ServerProtocol },
                    { "Headers", entry.HttpHeaders },
                    { "Form", entry.RequestEntity },
                    { "class", alt ? "alt" : null }
                });
                sb.Append(row);
                alt = !alt;
            }
            return sb.ToString();
        }

        private string GetContent()
        {
            if (BBEngine.Instance.Logger == null)
                return GetView("nolog", null);
            var entries = BBEngine.Instance.Logger.Query(query);
            if (entries == null)
                return GetView("nolog", null);
            if (!entries.LogEntries.Any())
                return GetView("empty", null);
            var tableRows = BuildTableRows(entries.LogEntries);
            var pager = new Pager(Context.Request.Url, query, entries);

            var unfiltered = query.Clone();
            unfiltered.Filter = unfiltered.FilterValue = null;
            string filter = Regex.Replace(query.Filter, "([a-z])([A-Z])", @"$1 $2");

            var content = GetView("log", new Dictionary<string, string>() {
                { "pager", pager.GetHtml() },
                { "rows", tableRows },
                { "firstEntry", entries.FirstEntry.ToString() },
                { "lastEntry", entries.LastEntry.ToString() },
                { "totalEntries", entries.TotalEntries.ToString() },
                { "filter", filter },
                { "nofilter", unfiltered.ToString() }
            });
            return content;
        }
    }
}
