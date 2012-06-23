using System;

namespace BadBehavior
{
    public interface IConfiguration
    {
        bool ReverseProxy { get; }
        System.Collections.Generic.IList<string> ReverseProxyAddresses { get; }
        string ReverseProxyHeader { get; }
    }
}
