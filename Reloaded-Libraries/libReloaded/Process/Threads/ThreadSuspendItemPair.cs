using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Process.Threads
{
    /// <summary>
    /// Pairs a flag of whether a thread is suspended with a thread object.
    /// </summary>
    public struct ThreadSuspendItemPair
    {
        /// <summary>
        /// This is true if the thread is suspended.
        /// </summary>
        public bool Suspended;

        /// <summary>
        /// The actual thread object which can be resumed/suspended.
        /// </summary>
        public System.Diagnostics.ProcessThread Thread;

        public ThreadSuspendItemPair(bool suspended, System.Diagnostics.ProcessThread thread)
        {
            Suspended = suspended;
            Thread = thread;
        }
    }
}
