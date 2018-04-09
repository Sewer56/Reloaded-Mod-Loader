/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
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

namespace Reloaded_GUI.Styles.Controls.Interfaces
{
    /// <summary>
    /// See note in Interface source.
    /// Interface which allows controls to specify whether they may be ignored by mouse or other user inputs.
    /// Interface elements should also override CreateParams, of access modifier protected. 
    /// </summary>
    internal interface IControlIgnorable
    {
        /// <summary>
        /// If set to true, the control ignores the mouse.
        /// </summary>
        bool IgnoreMouse { get; set; }

        /* 
        Note: This should be appended to all members implementing IControlIgnorable
         
        /// <summary>
        /// Overrides the information needed when the control is created or accessed to
        /// either ignore input on the label or not ignore input.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (IgnoreMouse) { cp.Style |= 0x08000000; }  // Enable WS_DISABLED
                return cp;
            }
        }
        */
    }
}
