﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BadBehavior.Validators;

namespace BadBehavior
{
    public class Validator
    {
        public IList<IValidation> Tests { get; private set; }

        public Validator()
        {
            this.Tests = new List<IValidation>();
        }

        public Validator(params IValidation[] tests)
        {
            this.Tests = tests.ToList();
        }

        public void Validate(HttpRequestBase request, IConfiguration configuration)
        {
            var package = new Package(request, configuration);

            foreach (var test in this.Tests) {
                if (test.Validate(package) == ValidationResult.Stop) return;
            }
        }
    }
}
