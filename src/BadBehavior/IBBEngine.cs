using System;
namespace BadBehavior
{
    public interface IBBEngine
    {
        event BadBehaviorEventHandler BadBehavior;
        IConfiguration Configuration { get; }
        void HandleError(System.Web.HttpApplication context, BadBehaviorException ex);
        System.Collections.Generic.IList<IRule> Rules { get; }
        void Throw(IRule validation, Package package, Error error);
        void ValidateRequest(System.Web.HttpRequestBase request);
    }
}
