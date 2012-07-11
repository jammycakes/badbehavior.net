using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehavior.Logging
{
    public interface ILogger
    {
        /// <summary>
        ///  Adds a new entry to the logs.
        /// </summary>
        /// <param name="entry"></param>

        void Log(LogEntry entry);

        /// <summary>
        ///  Clears all entries from the logs.
        /// </summary>

        void Clear();
    }
}
