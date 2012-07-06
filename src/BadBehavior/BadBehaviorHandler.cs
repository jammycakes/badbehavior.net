using System;
using System.Collections.Generic;
using System.Web;
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
            var content = this.GetContent();
            this.Context.Response.ContentType = "text/html";
            this.Context.Response.Write(content);
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

        private string GetContent()
        {
            if (BBEngine.Instance.LogReader == null)
                return GetView("nolog", null);
            return GetView("log", null);
        }
    }
}
