using ReloadedLauncher.Styles.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Themes
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
            foreach (Form windowForm in Global.WindowsForms) { KillAnimationsRecursive(windowForm); }
        }

        /// <summary>
        /// Recursively browses children to kill all winForm control animations.
        /// </summary>
        /// <param name="windowForm">The form of whose children's animations are to be terminated.</param>
        private static void KillAnimationsRecursive(Form windowForm)
        {
            /// Iterate over each control.
            foreach (Control control in windowForm.Controls)
            {
                // If the control has embedded controls (thus embeds child controls, apply theme to children.
                if (control.Controls.Count != 0) { foreach (Control controlEmbedded in control.Controls) { KillAnimationControl(controlEmbedded); } }

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
            if (control is IAnimatedControl)
            {
                // Cast to AnimatedControl
                IAnimatedControl animatedControl = (IAnimatedControl)control;

                // Stop ongoing messages.
                animatedControl.KillAnimations();
            }
        }
    }
}
