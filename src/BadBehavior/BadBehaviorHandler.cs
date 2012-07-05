using System;
using System.Web;

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

        private string GetContent()
        {
            return "<!doctype html><html><head><title>test</title></head><body><h1>test</h1></body></html>";
        }
    }
}
