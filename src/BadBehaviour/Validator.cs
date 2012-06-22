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
				where typeof(IValidation).IsAssignableFrom(type)
				let c = type.GetConstructor(Type.EmptyTypes)
				where c != null
				let test = c.Invoke(null) as IValidation
				where test != null
				select test
			);
		}

		public IList<IValidation> Tests { get; private set; }

		public Validator()
		{
			this.Tests = new List<IValidation>();
		}

		public Validator(IEnumerable<IValidation> tests)
		{
			this.Tests = tests.ToList();
		}

		public void Validate(HttpRequestBase request)
		{
			var package = new RequestPackage(request);

			foreach (var test in this.Tests) {
				if (test.Validate(package) == ValidationResult.Stop) return;
			}
		}
	}
}
