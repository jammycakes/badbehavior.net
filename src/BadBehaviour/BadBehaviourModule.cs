using System;
using System.Web;

namespace BadBehaviour
{
	public class BadBehaviourModule : IHttpModule
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
