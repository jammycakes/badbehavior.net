using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace BadBehavior
{
    public class BadBehaviorModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => {
                BBEngine.Instance.ValidateRequest(new HttpRequestWrapper(context.Request));
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
