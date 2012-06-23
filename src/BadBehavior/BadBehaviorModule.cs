using System;
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
				var request = new HttpRequestWrapper(context.Request);
				Validator.Instance.Validate(request);
			};
		}
	}
}
