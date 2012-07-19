using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using BadBehavior.Configuration;

namespace BadBehavior.Web
{
    public class BadBehaviorModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            BBEngine.Instance.Settings = SettingsLocator.FindSettings();

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
