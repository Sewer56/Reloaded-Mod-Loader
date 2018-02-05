using Reloaded.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReloadedLauncher.Utilities.Windows
{
    /// <summary>
    /// Applies compositing to a Windows Forms window by enabling the WS_EX_COMPOSITED
    /// extended window style.
    /// </summary>
    public static class WindowCompositing
    {
        /// <summary>
        /// Applies compositing to a Windows Forms window by enabling the WS_EX_COMPOSITED
        /// extended window style.
        /// </summary>
        /// <param name="windowsForm">The Windows form whose compositing is to be applied.</param>
        public static void ApplyCompositing(Form windowsForm)
        {
            // Retrieves the extended window style.
            long extendedWindowStyle = (long)WinAPI.WindowStyles.GetWindowLongPtr(windowsForm.Handle, WinAPI.WindowStyles.Constants.GWL_EXSTYLE);

            // Append WS_EX_COMPOSITED window style.
            extendedWindowStyle |= WinAPI.WindowStyles.Constants.WS_EX_COMPOSITED;

            // Set window style.
            WinAPI.WindowStyles.SetWindowLongPtr(new HandleRef(windowsForm, windowsForm.Handle), WinAPI.WindowStyles.Constants.GWL_EXSTYLE, (IntPtr)extendedWindowStyle);
        }
    }
}
