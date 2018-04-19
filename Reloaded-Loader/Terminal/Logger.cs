/*
    [Reloaded] Mod Loader Application Loader
    The main loader, which starts up an application loader and using DLL Injection methods
    provided in the main library initializes modifications for target games and applications.
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded_Loader.Terminal
{
    /// <summary>
    /// The logger class provides the functionality used to write logs of all text printed to the console 
    /// to a user specified log file on the filesystem.
    /// </summary>
    static class Logger
    {
        /// <summary>
        /// File stream used to append text to be printed out to a log file.
        /// </summary>
        private static StreamWriter _logFileStream;

        /// <summary>
        /// Sets up the logger to enable the printing of text to a file at a specified
        /// location.
        /// </summary>
        /// <param name="location"></param>
        public static void Setup(string location)
        {
            _logFileStream = new StreamWriter(new FileStream(location, FileMode.Create));
            _logFileStream.AutoFlush = true;
        }

        /// <summary>
        /// Closes the internal log fileStream.
        /// </summary>
        public static void Close()
        {
            _logFileStream.BaseStream.Close();
            _logFileStream.Close();
            _logFileStream = null;
        }

        /// <summary>
        /// Appends a message to the user log file.
        /// </summary>
        /// <param name="message">The message to be appended to the log file.</param>
        public static void Append(string message)
        {
            _logFileStream?.WriteLine(message);
        }
    }
}
