using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BadBehavior.Validators;

namespace BadBehavior
{
	public class Validator
	{
		public static Validator Instance { get; set; }

		public static Configuration Configuration { get; set; }

		static Validator()
		{
			Instance = new Validator(
				new CloudFare(),
				new WhiteList(),
				new BlackList(),
				new BlackHole(),
				new Protocol(),
				new Cookies(),
				new MiscHeaders(),
				new SearchEngine(),
				new MovableType(),
				new Browser(),
				new Post()
			);
			Configuration = new Configuration();
		}

		public IList<IValidation> Tests { get; private set; }

		public Validator()
		{
			this.Tests = new List<IValidation>();
		}

		public Validator(params IValidation[] tests)
		{
			this.Tests = tests.ToList();
		}

		public void Validate(HttpRequestBase request)
		{
			var package = new Package(request, Configuration);

			foreach (var test in this.Tests) {
				if (test.Validate(package) == ValidationResult.Stop) return;
			}
		}
	}
}
