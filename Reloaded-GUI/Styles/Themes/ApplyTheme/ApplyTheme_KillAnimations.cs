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

using System.Windows.Forms;
using Reloaded_GUI.Styles.Animation;

namespace Reloaded_GUI.Styles.Themes.ApplyTheme
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This file handles the anmimation killing.
    /// </summary>
    public static partial class ApplyTheme
    {
        /// <summary>
        /// Stops animations for all animated winForms contorls.
        /// </summary>
        public static void KillAnimations()
        {
            // For each currently initialized Windows Form.
            foreach (Form windowForm in Bindings.WindowsForms)
                KillAnimationsRecursive(windowForm);
        }

        /// <summary>
        /// Recursively browses children to kill all winForm control animations.
        /// </summary>
        /// <param name="windowForm">The form of whose children's animations are to be terminated.</param>
        private static void KillAnimationsRecursive(Form windowForm)
        {
            // Iterate over each control.
            foreach (Control control in windowForm.Controls)
            {
                // If the control has embedded controls (thus embeds child controls, apply theme to children.
                if (control.Controls.Count != 0)
                    foreach (Control controlEmbedded in control.Controls) KillAnimationControl(controlEmbedded);

                // Apply the theme.
                KillAnimationControl(control);
            }
        }

        /// <summary>
        /// Kills animations for a specified passed in control if it implements the IAnimatedControl interface.
        /// </summary>
        /// <param name="control">The control whose animation is to be killed.</param>
        private static void KillAnimationControl(Control control)
        {
            // If it is an animated control
            if (control is IAnimatedControl animatedControl)
            {
                // Stop ongoing messages.
                animatedControl.KillAnimations();
            }
        }
    }
}
