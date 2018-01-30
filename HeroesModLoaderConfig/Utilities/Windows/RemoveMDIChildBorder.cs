using SonicHeroes.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Utilities.Windows
{
    public static class RemoveMDIChildBorder
    {
        /// <summary>
        /// Removes all of the sunken 3D borders from the client MDI Children
        /// of an MDI Parent form.
        /// </summary>
        /// <param name="winForm">The form whose MDI Children are to have their sunken borders removed.</param>
        /// <param name="showBorders">Set to true if the sunken 3D borders are to be shown, else false.</param>
        public static void SetBevel(this Form winForm, bool showBorders)
        {
            // For each control within the form (MDI Children are also regarded as controls)
            foreach (Control control in winForm.Controls)
            {
                // Try to cast the control as an MDI Child Form
                MdiClient mdiCLientForm = control as MdiClient;

                // If it succeeds, remove the bezel.
                if (mdiCLientForm != null)
                {
                    // Get the window properties.
                    long windowLong = (long)WinAPI.WindowStyles.GetWindowLongPtr(control.Handle, WinAPI.WindowStyles.Constants.GWL_EXSTYLE);

                    // Remove (or append) the border flags.
                    if (showBorders) { windowLong |= WinAPI.WindowStyles.Constants.WS_EX_CLIENTEDGE; }
                    else { windowLong &= ~WinAPI.WindowStyles.Constants.WS_EX_CLIENTEDGE; }

                    // Set the new extended window flags.
                    WinAPI.WindowStyles.SetWindowLongPtr(new HandleRef(control, control.Handle), WinAPI.WindowStyles.Constants.GWL_EXSTYLE, (IntPtr)windowLong);
                }
            }
        }
    }
}
