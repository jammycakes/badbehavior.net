using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior
{
    public class BadBehaviorEventArgs : EventArgs
    {
        public Error Error { get; private set; }

        public Package Package { get; private set; }

        public BadBehaviorEventArgs(BadBehaviorException ex)
        {
            this.Error = ex.Error;
            this.Package = ex.Package;
        }

        public BadBehaviorEventArgs(Package package, Error error)
        {
            this.Package = package;
            this.Error = error;
        }
    }

    public delegate void BadBehaviorEventHandler(object sender, BadBehaviorEventArgs e);
}
