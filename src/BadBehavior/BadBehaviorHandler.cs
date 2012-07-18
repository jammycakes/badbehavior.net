using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BadBehavior.Admin;
using BadBehavior.Logging;
using BadBehavior.Rules;
using BadBehavior.Util;

namespace BadBehavior
{
    public class BadBehaviorHandler : IHttpHandler
    {
        public HttpContextBase Context { get; set; }

        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.Context = new HttpContextWrapper(context);
            if (!BBEngine.Instance.Settings.AllowRemoteLogViewing) {
                AssertLocal();
            }

            var content = this.GetContent();
            this.Context.Response.ContentType = "text/html";
            this.Context.Response.Write(content);
        }

        private void AssertLocal()
        {
            if (!this.Context.Request.IsLocal)
                throw new HttpException
                    (404, "The resource you requested was not found on this server.");
        }

        #endregion

        const string templatePrefix = "BadBehavior.Admin.templates.";
        static readonly Template master = Template.FromResource(templatePrefix + "_master.html");

        private string GetView(string viewName, IDictionary<string, string> parameters)
        {
            parameters = parameters ?? new Dictionary<string, string>();
            var view = Template.FromResource(templatePrefix + viewName + ".html");
            var content = view.Process(parameters);
            return master.Process(new Dictionary<string, string> {
                { "content", content }
            });
        }

        private string BuildTableRows(IEnumerable<LogEntry> entries)
        {
            var tpl = Template.FromResource("BadBehavior.Admin.templates.row.html");
            var sb = new StringBuilder();
            bool alt = false;
            foreach (var entry in entries) {
                string row = tpl.Process(new Dictionary<string, string>() {
                    { "Date", entry.Date.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "IP", entry.IP.ToString() },
                    { "LogMessage", Errors.Lookup(entry.Key).Log },
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
            var query = new LogQuery(Context.Request.QueryString);
            var entries = BBEngine.Instance.Logger.Query(query);
            if (entries == null)
                return GetView("nolog", null);
            if (!entries.LogEntries.Any())
                return GetView("empty", null);
            var tableRows = BuildTableRows(entries.LogEntries);
            var pager = new Pager(Context.Request.Url, query, entries);

            var content = GetView("log", new Dictionary<string, string>() {
                { "pager", pager.GetHtml() },
                { "rows", tableRows },
                { "firstEntry", entries.FirstEntry.ToString() },
                { "lastEntry", entries.LastEntry.ToString() },
                { "totalEntries", entries.TotalEntries.ToString() }
            });
            return content;
        }
    }
}
