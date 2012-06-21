using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BadBehaviour
{
	public class Validator
	{
		public static Validator Instance { get; set; }

		static Validator()
		{
			Instance = new Validator();
		}

		public void Validate(HttpRequestBase request)
		{
		}
	}
}
