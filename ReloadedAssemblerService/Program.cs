/*
    [Reloaded] Mod Loader FASM Server
    A websocket server based off of libReloaded [Reloaded] main library,
    allowing x86/x64 FASM compatible mnemonics to be assembled.
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

/*
   Purpose:
   
   Allows x64, other architecture processes to assemble mnemonics via the
   FASM assembler as long as the machine supports running x86 code in any fashion.

   This is important for Reloaded mods that exist particularly in the target space
   of x64 processes, as the assembler cannot normally be used within that context
   and starting FASM.exe as a process for singular assembly jobs causes additional
   unsightly thread creation overhead.
 */

using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace ReloadedAssembler
{   
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Starts off the service in question.
        /// </summary>
        public static void Main(string[] arguments)
        {
            // Check if an instance of this program already exists.
            // If it does, exit.
            if (Process.GetProcessesByName("ReloadedAssembler").Length > 1)
            { Environment.Exit(0); }

            FasmServer fasmServer = new FasmServer();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
