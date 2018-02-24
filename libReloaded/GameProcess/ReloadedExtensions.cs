using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Provides various uncategorized extension classes which don't belong in any of the other classes in question.
    /// </summary>
    public static class ReloadedExtensions
    {
        /// <summary>
        /// Resumes a suspended thread supplied by the handle.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);

        /// <summary>
        /// Suspends a running thread supplied by the handle.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        /// <summary>
        /// Resumes the first/main/primary thread assigned to the Reloaded Process object.
        /// </summary>
        public static void ResumeFirstThread(this ReloadedProcess reloadedProcess)
        {
            ResumeThread(reloadedProcess.threadHandle);
        }

        /// <summary>
        /// Suspends the first/main/primary thread assigned to the Reloaded Process object.
        /// </summary>
        public static void SuspendFirstThread(this ReloadedProcess reloadedProcess)
        {
            SuspendThread(reloadedProcess.threadHandle);
        }
    }
}
