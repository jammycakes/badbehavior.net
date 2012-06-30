using System;
namespace BadBehavior
{
    public interface IBBEngine
    {
        event BadBehaviorEventHandler BadBehavior;
        void HandleError(System.Web.HttpApplication context, BadBehaviorException ex);
        System.Collections.Generic.IList<IRule> Rules { get; }
        void Raise(IRule validation, Package package, Error error);
        void ValidateRequest(System.Web.HttpRequestBase request);
        ISettings Settings { get; }
    }
}
