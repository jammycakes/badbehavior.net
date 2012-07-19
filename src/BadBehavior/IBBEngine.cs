using System.Collections.Generic;
using BadBehavior.Logging;

namespace BadBehavior
{
    public interface IBBEngine
    {
        ILogger Logger { get; set; }
        IList<IRule> Rules { get; }
        ISettings Settings { get; set; }

        void HandleError(System.Web.HttpApplication context, BadBehaviorException ex);
        void Raise(Package package, Error error);
        void ValidateRequest(System.Web.HttpRequestBase request);
        void RaiseStrict(Package package, Error error);

        event BadBehaviorEventHandler BadBehavior;
    }
}
