using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using BadBehavior.Configuration;

namespace BadBehavior
{
    public class BadBehaviorModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            BBEngine.Instance.Settings = FindSettings();

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

        public ISettings FindSettings()
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var thisAssembly = this.GetType().Assembly;

            var types = from assembly in assemblies
                        where assembly != thisAssembly
                        from type in assembly.GetTypes()
                        where typeof(ISettings).IsAssignableFrom(type)
                            && type.GetConstructor(Type.EmptyTypes) != null
                        select type;
            var settings = types.Select(x => Activator.CreateInstance(x))
                .Cast<ISettings>().FirstOrDefault();

            return settings
                ?? ConfigurationManager.GetSection("badBehavior") as ISettings
                ?? new BadBehaviorConfigurationSection();
        }
    }
}
