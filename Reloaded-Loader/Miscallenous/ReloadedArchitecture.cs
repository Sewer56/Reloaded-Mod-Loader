using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.GameProcess;

namespace Reloaded_Loader.Miscallenous
{
    /// <summary>
    /// Verifies whether
    /// </summary>
    public static class ReloadedArchitecture
    {
        /// <summary>
        /// Verifies whether the architecture of a Reloaded Process matches the architecture of
        /// the current program.
        /// </summary>
        public static bool CheckArchitectureMatch(this ReloadedProcess reloadedProcess)
        {
            // Check if Process is x64.
            Native.IsWow64Process(reloadedProcess.processHandle, out Program.isGame32Bit);

            // Compare against current process.
            return Program.isGame32Bit == (! Environment.Is64BitProcess);
        }
    }
}
