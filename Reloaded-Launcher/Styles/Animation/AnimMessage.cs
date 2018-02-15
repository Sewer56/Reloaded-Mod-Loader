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


namespace ReloadedLauncher.Styles.Animation
{
    /// <summary>
    /// Provides a struct that couples a control alongside a flag which signals whether the current animation
    /// playback should be terminated for next animation.
    /// </summary>
    public class AnimMessage
    {
        /// <summary>
        /// The Windows Forms Control (or any object Implementing BackColor, ForeColor) that is to be animated.
        /// </summary>
        public object Control { get; set; }

        /// <summary>
        /// The flag which defines whether the animation should be played.
        /// If set to false, the animation will permanently cease.
        /// </summary>
        public bool PlayAnimation { get; set; }

        /// <summary>
        /// Instantiates an animation message which defines a flag declaring whether
        /// the animation should be immediately halted and the Windows Forms control that
        /// should be animated.
        /// </summary>
        /// <param name="control">Defines the control that is intended to be animated.</param>
        public AnimMessage(object control)
        {
            Control = control;
            PlayAnimation = true;
        }

        /// <summary>
        /// Default constructor, not worth using.
        /// </summary>
        public AnimMessage() { PlayAnimation = true; }
    }
}
