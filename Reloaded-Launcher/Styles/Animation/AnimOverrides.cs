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

using System;

namespace ReloadedLauncher.Styles.Animation
{
    /// <summary>
    /// Specifies enumerables, methods and general structs, variables, etc. used for event overrides.
    /// e.g. diverting OnEnter and OnLeave events for mouse buttons.
    /// </summary>
    public class AnimOverrides
    {
        /// <summary>
        /// Specifies overrides for the mouse enter events for custom animated windows forms
        /// controls. 
        /// </summary>
        [Flags]
        public enum MouseEnterOverride
        {
            /// <summary>
            /// Run default mouse enter events.
            /// </summary>
            None,
            /// <summary>
            /// Interpolates the BackColor from the current original BackColor to a new custom specified BackColor.
            /// </summary>
            BackColorInterpolate,
            /// <summary>
            /// Interpolates the ForeColor from the current original ForeColor to a new custom specified ForeColor.
            /// </summary>
            ForeColorInterpolate
        }

        /// <summary>
        /// Specifies overrides for the mouse leave events for custom animated windows forms
        /// controls. 
        /// </summary>
        [Flags]
        public enum MouseLeaveOverride
        {
            /// <summary>
            /// Run default mouse enter events.
            /// </summary>
            None,
            /// <summary>
            /// Interpolates the BackColor from the current original BackColor to a new custom specified BackColor.
            /// </summary>
            BackColorInterpolate,
            /// <summary>
            /// Interpolates the ForeColor from the current original ForeColor to a new custom specified ForeColor.
            /// </summary>
            ForeColorInterpolate
        }
    }
}
