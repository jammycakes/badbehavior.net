using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BadBehavior;

namespace Website
{
    public class BadBehaviorConfigurator : IConfigurator
    {
        public void Configure(BBEngine engine)
        {
            var settings = engine.Settings;
            settings.Debug = true;
        }
    }
}