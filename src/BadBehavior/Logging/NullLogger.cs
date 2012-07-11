using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    public class NullLogger : ILogger
    {
        public void Log(LogEntry entry)
        {
        }

        public void Clear()
        {
        }
    }
}
