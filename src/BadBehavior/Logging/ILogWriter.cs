using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    public interface ILogWriter
    {
        /// <summary>
        ///  Adds a new entry to the logs.
        /// </summary>
        /// <param name="entry"></param>

        void Log(LogEntry entry);
    }
}
