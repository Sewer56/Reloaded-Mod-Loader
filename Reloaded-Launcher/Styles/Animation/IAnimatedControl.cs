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
    /// Defines an interface which contains the necessary components for
    /// performing simple animation with the mod loader configuration tool.
    /// </summary>
    public interface IAnimatedControl
    {
        /// <summary>
        /// Calls the object's normally internal protected
        /// OnMouseEnter if no override is set.
        /// </summary>
        void OnMouseEnterWrapper(EventArgs e);

        /// <summary>
        /// Calls the object's normally internal protected
        /// OnMouseLeave if no override is set.
        /// </summary>
        void OnMouseLeaveWrapper(EventArgs e);

        /// <summary>
        /// Should contain an AnimProperties struct containing all of
        /// the animation properties.
        /// </summary>
        AnimProperties AnimProperties { get; set; }

        /// <summary>
        /// Stops all currently ongoing animations.
        /// </summary>
        void KillAnimations();
    }
}
