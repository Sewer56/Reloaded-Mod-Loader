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
    /// Declares the variables a control must posess if it is to be potentially used as a decoration box.
    /// Allows/Disallows the capturing of children controls if they are in the region of the 
    /// </summary>
    internal interface IDecorationBox
    {
        /// <summary>
        /// Declares whether the decoration box should capture the children controls 
        /// </summary>
        bool CaptureChildren { get; set; }
    }
}
