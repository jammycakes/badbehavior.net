using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using BadBehavior.Configuration;

namespace BadBehavior
{
    /* ====== BadBehaviorModule class ====== */

    /// <summary>
    ///  The HTTP module responsible for vetting all your HTTP requests for
    ///  nefarious activity.
    /// </summary>

    public class BadBehaviorModule : IHttpModule
    {
        /// <summary>
        ///  Disposes of any unmanaged resources used by the HTTP module.
        /// </summary>

        public void Dispose()
        {
        }

        /// <summary>
        ///  Initialises the HTTP module and prepares it to handle requests.
        /// </summary>
        /// <param name="context"></param>

        public void Init(HttpApplication context)
        {
            var configurator = ConfiguratorLocator.Find();
            BBEngine.Instance.Configure(new IConfigurator[] { configurator });

            context.BeginRequest += (sender, e) => {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                try {
                    BBEngine.Instance.ValidateRequest(new HttpRequestWrapper(context.Request));
                }
                finally {
                    sw.Stop();
                    context.Context.Trace.Write
                        ("BadBehavior", "Analysis completed in: " + sw.Elapsed.TotalMilliseconds + " milliseconds");
                }
            };

            context.Error += (sender, e) => {
                var ex = context.Server.GetLastError() as BadBehaviorException;
                if (ex != null) {
                    BBEngine.Instance.HandleError(context, ex);
                }
            };
        }
    }
}
