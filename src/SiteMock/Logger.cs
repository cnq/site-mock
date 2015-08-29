using System.Collections.Generic;
using System.Diagnostics;

namespace SiteMock
{
    public class Logger
    {
        private readonly List<string> _entries;
        private static readonly object Lock = new object();

        public Logger()
        {
            _entries = new List<string>();
        }

        public void Log(string message, params object[] args)
        {
            _entries.Add(string.Format(message, args));
        }

        public void Flush()
        {
            lock (Lock)
            {
                foreach (var entry in _entries)
                {
                    Trace.TraceInformation(entry);
                }
            }
          
        }
    }
}
