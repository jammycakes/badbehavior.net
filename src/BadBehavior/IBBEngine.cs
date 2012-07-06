using System;
using BadBehavior.Logging;

namespace BadBehavior
{
    public interface IBBEngine
    {
        event BadBehaviorEventHandler BadBehavior;
        void HandleError(System.Web.HttpApplication context, BadBehaviorException ex);
        System.Collections.Generic.IList<IRule> Rules { get; }
        void Raise(Package package, Error error);
        void ValidateRequest(System.Web.HttpRequestBase request);
        ISettings Settings { get; }
        void RaiseStrict(Package package, Error error);
        ILogWriter Logger { get; set; }
        ILogReader LogReader { get; set; }
    }
}
