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
    /// Defines the different individual message types accepted by the Mod Loader Server.
    /// This is all [Reloaded] Mod Loader specific.
    /// </summary>
    public static class MessageTypes
    {
        /// <summary>
        /// Defines the individual message types that can be sent towards the mod loader server.
        /// It's recommended that you actually go to the definition of this in your IDE, everything is nicely formatted there.
        /// </summary>
        public enum MessageType : ushort
        {
            /// <summary>
            /// Expects Response:   False
            /// Definition:         Confirmation of operation being performed successfully.
            /// </summary>
            Okay = 0x0,

            /// <summary>
            /// Expects Response:   True 
            /// Definition:         Assembles your x86 mnemonics sent to the mod loader server. Powered by FASM.NET.
            /// Data:               Returns the bytes representing the x86 mnemonics given. 
            /// Notes:              Return data is 100% raw. Not a message struct.
            /// </summary>
            AssembleX86 = 0x1,

            /// <summary>
            /// Expects Response:   True/False
            /// Definition:         Prints message to mod loader's command line, regular style. 
            /// Data:               ASCII Encoded String, e.g. Encoding.ASCII.GetBytes("Ayylmao");
            /// </summary>
            PrintText = 0x2,

            /// <summary>
            /// Expects Response:   True/False
            /// Definition:         Prints message to mod loader's command line, in yellow/warning style. 
            /// Data:               ASCII Encoded String, e.g. Encoding.ASCII.GetBytes("Ayylmao");
            /// </summary>
            PrintWarning = 0x3,

            /// <summary>
            /// Expects Response:   True/False
            /// Definition:         Prints message to mod loader's command line, in blue/info style. 
            /// Data:               ASCII Encoded String, e.g. Encoding.ASCII.GetBytes("Ayylmao");
            /// </summary>
            PrintInfo = 0x4,

            /// <summary>
            /// Expects Response:   True/False
            /// Definition:         Prints message to mod loader's command line, in red/error style. 
            /// Data:               ASCII Encoded String, e.g. Encoding.ASCII.GetBytes("Ayylmao");
            /// </summary>
            PrintError = 0x5,
        }
    }
}
