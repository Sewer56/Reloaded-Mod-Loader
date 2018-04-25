/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.Threading.Tasks;
using static Reloaded.Process.Native.Native;

namespace Reloaded.DirectX
{
    /// <summary>
    /// Provides various common functions used within the realm of DirectX hooking that
    /// translate between different individual versions of DirectX.
    /// </summary>
    public static class DXHookCommon
    {
        /// <summary>
        /// Determines the version of DirectX used in the current process by
        /// evaluating the currently loaded in DirectX modules.
        /// </summary>
        /// <returns></returns>
        public static async Task<Direct3DVersion> DetermineDirectXVersion()
        {
            // Store the amount of attempts taken at hooking DirectX for a process.
            int attempts = 0;
            int delayTimeMilliseconds = 100;
            int timeoutMillisecondsHooking = 5000;
            int timeoutMillisecondsWindow = 20000;

            // Wait until the process spawns a window.
            while (true)
            {
                // Add attempt
                attempts++;

                // Check if there is a main window spawned for this process.
                if ((int)Bindings.TargetProcess.GetProcessFromReloadedProcess().MainWindowHandle != 0)
                { break; }

                // Check timeout.
                if (attempts * delayTimeMilliseconds > timeoutMillisecondsWindow)
                {
                    Bindings.PrintError("libReloaded Hooking: [Timeout] No window has been found for the application.");
                    return Direct3DVersion.Null;
                }

                // Check every X milliseconds.
                await Task.Delay(delayTimeMilliseconds);
            }

            // Loop until DirectX module found.
            while (true)
            {
                // Add attempt
                attempts++;

                if ((long)GetModuleHandle("d3d9.dll") != 0) { return Direct3DVersion.Direct3D9; }
                if ((long)GetModuleHandle("d3d10.dll") != 0) { return Direct3DVersion.Direct3D10; }
                if ((long)GetModuleHandle("d3d10_1.dll") != 0) { return Direct3DVersion.Direct3D10_1; }
                if ((long)GetModuleHandle("d3d11.dll") != 0) { return Direct3DVersion.Direct3D11; }
                if ((long)GetModuleHandle("d3d11_1.dll") != 0) { return Direct3DVersion.Direct3D11_1; }
                if ((long)GetModuleHandle("d3d11_2.dll") != 0) { return Direct3DVersion.Direct3D11_2; }
                if ((long)GetModuleHandle("d3d11_3.dll") != 0) { return Direct3DVersion.Direct3D11_3; }
                if ((long)GetModuleHandle("d3d11_4.dll") != 0) { return Direct3DVersion.Direct3D11_4; }

                // Check timeout.
                if (attempts * delayTimeMilliseconds > timeoutMillisecondsHooking)
                {
                    Bindings.PrintError(
                        "libReloaded Hooking: DirectX module not found, the application is either not" +
                        "a DirectX application or uses an unsupported version of DirectX.");
                    return Direct3DVersion.Null;
                }

                // Check every X milliseconds.
                await Task.Delay(delayTimeMilliseconds);
            }
        }
    }
}
