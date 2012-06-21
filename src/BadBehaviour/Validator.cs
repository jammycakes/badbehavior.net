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
			Instance = new Validator(
				from type in typeof(Validator).Assembly.GetTypes()
				where typeof(ITest).IsAssignableFrom(type)
				let c = type.GetConstructor(Type.EmptyTypes)
				where c != null
				let test = c.Invoke(null) as ITest
				where test != null
				select test
			);
		}

		public IList<ITest> Tests { get; private set; }

		public Validator()
		{
			this.Tests = new List<ITest>();
		}

		public Validator(IEnumerable<ITest> tests)
		{
			this.Tests = tests.ToList();
		}

		public void Validate(HttpRequestBase request)
		{
			foreach (var test in this.Tests) {
				if (test.Test(request)) {
					throw new BadBehaviourException(test, request);
				}
			}
		}
	}
}
