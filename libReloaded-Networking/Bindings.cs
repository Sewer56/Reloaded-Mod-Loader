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

namespace libReloaded_Networking
{
    /// <summary>
    /// Method/delegate/variable declarations for internal library functions to use. 
    /// Subscribe to these if you want to receive messages from libReloaded-Networking.
    /// If you are using libReloaded, you do not require to use this class, subscribe to libReloaded instead.
    /// </summary>
    public static class Bindings
    {
        /// <summary>
        /// Function used to print normal text to a file/screen/buffer.
        /// In the case of mod loader mods, it is the console.
        /// </summary>
        public static PrintMessageDelegate PrintText { get; set; }

        /// <summary>
        /// Function used to print info text to a file/screen/buffer.
        /// In the case of mod loader mods, it is the console.
        /// </summary>
        public static PrintMessageDelegate PrintInfo { get; set; }

        /// <summary>
        /// Function used to print warning text to a file/screen/buffer.
        /// In the case of mod loader mods, it is the console.
        /// </summary>
        public static PrintMessageDelegate PrintWarning { get; set; }

        /// <summary>
        /// Function used to print error text to a file/screen/buffer.
        /// In the case of mod loader mods, it is the console.
        /// </summary>
        public static PrintMessageDelegate PrintError { get; set; }

        /// <summary>
        /// Delegate used for functions which involve the printing of a message to a file/screen/stream/buffer.
        /// </summary>
        /// <param name="message">The message to be printed to the file/screen/stream/buffer.</param>
        public delegate void PrintMessageDelegate(string message);
    }
}
