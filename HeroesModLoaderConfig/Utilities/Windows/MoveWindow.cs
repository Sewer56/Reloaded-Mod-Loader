using System;
using System.Runtime.InteropServices;

namespace HeroesModLoaderConfig.Utilities.Windows
{
    /// <summary>
    /// The MoveWindow class allows for the window in which a control is held to be moved.
    /// that simple, really...
    /// </summary>
    public static class MoveWindow
    {
        /// <summary>
        /// Sends the specified message to a window or windows. 
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="msg">The message to be sent. For lists of the system-provided messages, see System-Defined Messages on MSDN.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// Releases the mouse capture from a window in the current thread and restores normal mouse input processing. 
        /// A window that has captured the mouse receives all mouse input, regardless of the position of the cursor, 
        /// except when a mouse button is clicked while the cursor hot spot is in the window of another thread. 
        /// </summary>
        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();

        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) 
        /// or when the user chooses the maximize button, minimize button, restore button, or close button.
        /// </summary>
        private const int WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// wParam that moves the window.
        /// </summary>
        /// <remarks>
        /// Why does this even work? This is undocumented on MSDN when it comes to 
        /// SYSCOMMAND messages, it also appears that all 0xF01X where X is a value from 1-F work.
        /// Closest documented SYSCOMMAND param is SC_MOVE, 0xF010.
        /// </remarks>
        private const int MOUSE_MOVE = 0xF012;

        /// <summary>
        /// Moves the window if executed, accepts the windows form that is
        /// to be moved.
        /// </summary>
        /// <param name="handle">The handle to the form, event or control that is to be moved.</param>
        public static void MoveTheWindow(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, WM_SYSCOMMAND, MOUSE_MOVE, 0);
        }
    }
}
