/*
    [Reloaded] Mod Loader
    A universal, powerful multi-game, multi-process mod loader based on DLL Injection. 
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
    [Reloaded] is not associated with the warez group which goes by the same name.
    If you like the Mod Loader, feel free to contribute.
*/

/*
    Note: Application version may be set by modifying <Major>, <Minor> and <Build> properties in the project file. (.csproj)
    Visual Studio: Unload project, then an edit button shall appear.
*/
using System;
using System.Windows.Forms;

namespace ReloadedLauncher
{
    static class Program
    {
        /// <summary>
        /// Defines the initializer class which starts up the Windows Forms application.
        /// </summary>
        static Initializer initializer;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable Visual Styles
            Application.EnableVisualStyles();

            // Set Compatible Text Rendering Defs.
            Application.SetCompatibleTextRenderingDefault(false);

            // Call the Program Initializer.
            initializer = new Initializer();
        }
    }
}
