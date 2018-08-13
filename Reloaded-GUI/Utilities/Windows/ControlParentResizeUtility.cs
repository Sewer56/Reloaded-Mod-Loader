using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reloaded_GUI.Utilities.Windows
{
    /// <summary>
    /// The ControlParentResizeUtility allows you to pass a control and hit test the parent of the control.
    /// If the mouse is within specified radius of the parent's borders, the parent form may be resized.
    /// </summary>
    public class ControlParentResizeUtility
    {
        /// <summary>
        /// Contains a copy of the control passed into the constructor of the method.
        /// </summary>
        private Control _control;

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
        /// Sends the specified message to a window or windows. 
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="msg">The message to be sent. For lists of the system-provided messages, see System-Defined Messages on MSDN.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Releases the mouse capture from a window in the current thread and restores normal mouse input processing. 
        /// A window that has captured the mouse receives all mouse input, regardless of the position of the cursor, 
        /// except when a mouse button is clicked while the cursor hot spot is in the window of another thread. 
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        /*
            Getters for the parent form's borders. 
        */
        const int parentHitTestBorderSize = 15;

        Rectangle Top => new Rectangle(0, 0, _control.FindForm().ClientSize.Width, parentHitTestBorderSize);
        Rectangle Left => new Rectangle(0, 0, parentHitTestBorderSize, _control.FindForm().ClientSize.Height);
        Rectangle Bottom => new Rectangle(0, _control.FindForm().ClientSize.Height - parentHitTestBorderSize, _control.FindForm().ClientSize.Width, parentHitTestBorderSize);
        Rectangle Right => new Rectangle(_control.FindForm().ClientSize.Width - parentHitTestBorderSize, 0, parentHitTestBorderSize, _control.FindForm().ClientSize.Height);

        Rectangle TopLeft => new Rectangle(0, 0, parentHitTestBorderSize, parentHitTestBorderSize);
        Rectangle TopRight => new Rectangle(_control.FindForm().ClientSize.Width - parentHitTestBorderSize, 0, parentHitTestBorderSize, parentHitTestBorderSize);
        Rectangle BottomLeft => new Rectangle(0, _control.FindForm().ClientSize.Height - parentHitTestBorderSize, parentHitTestBorderSize, parentHitTestBorderSize);
        Rectangle BottomRight => new Rectangle(_control.FindForm().ClientSize.Width - parentHitTestBorderSize, _control.FindForm().ClientSize.Height - parentHitTestBorderSize, parentHitTestBorderSize, parentHitTestBorderSize);

        private enum ParentHitTestResult
        {
            Null,
            Top,
            Left,
            Bottom,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        /// <summary>
        /// Moves the window if executed, accepts the windows form that is
        /// to be moved.
        /// </summary>
        /// <param name="handle">The handle to the form, event or control that is to be moved.</param>
        /// <param name="result">Tells us precisely which corner the mouse is at, if any.</param>
        private void ResizeWindow(IntPtr handle, ParentHitTestResult result)
        {
            if (result != ParentHitTestResult.Null)
            {
                // I can't find the documentataion on those variations of the SC_MOVE, I got them by trial + error
                // 000 = Mouse Center Reset
                // 001 = Left
                // 002 = Right
                // 003 = Top
                // 004 = TopLeft
                // 005 = TopRight
                // 006 = Bottom
                // 007 = BottomLeft
                // 008 = BottomRight

                if (result == ParentHitTestResult.Left)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF001, IntPtr.Zero);
                if (result == ParentHitTestResult.Right)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF002, IntPtr.Zero);
                if (result == ParentHitTestResult.Top)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF003, IntPtr.Zero);
                if (result == ParentHitTestResult.TopLeft)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF004, IntPtr.Zero);
                if (result == ParentHitTestResult.TopRight)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF005, IntPtr.Zero);
                if (result == ParentHitTestResult.Bottom)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF006, IntPtr.Zero);
                if (result == ParentHitTestResult.BottomLeft)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF007, IntPtr.Zero);
                if (result == ParentHitTestResult.BottomRight)
                    SendMessage(handle, WM_SYSCOMMAND, (IntPtr)0xF008, IntPtr.Zero);
            }
            ReleaseCapture();
        }

        /// <summary>
        /// The ControlParentResizeUtility allows you to pass a control and hit test the parent of the control.
        /// If the mouse is within specified radius of the parent's borders, the parent form may be resized.
        /// </summary>
        /// <param name="control">The control whose parent will be hit tested on mouse move.</param>
        public ControlParentResizeUtility(Control control)
        {
            _control = control;
            _control.MouseMove += ProcessMouseMove;
        }

        private void ProcessMouseMove(object sender, MouseEventArgs e)
        {
            var result = SetCurrentCursor();
            if (e.Button == MouseButtons.Left)
            {
                ResizeWindow(_control.FindForm().Handle, result);
            }
        }

        /// <summary>
        /// Sets the appropriate cursor style for the current mouse position.
        /// </summary>
        private ParentHitTestResult SetCurrentCursor()
        {
            // Get hit test result & act accordingly.
            ParentHitTestResult result = HitTestParentControl();
            if (result != ParentHitTestResult.Null)
            {
                if (result == ParentHitTestResult.TopLeft || result == ParentHitTestResult.BottomRight)
                    Cursor.Current = Cursors.SizeNWSE;
                else if (result == ParentHitTestResult.BottomLeft || result == ParentHitTestResult.TopRight)
                    Cursor.Current = Cursors.SizeNESW;
                else if (result == ParentHitTestResult.Left || result == ParentHitTestResult.Right)
                    Cursor.Current = Cursors.SizeWE;
                else if (result == ParentHitTestResult.Top || result == ParentHitTestResult.Bottom)
                    Cursor.Current = Cursors.SizeNS;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }

            return result;
        }

        /// <summary>
        /// Checks which edge/corner of the parent form, if any - the mouse is on.
        /// </summary>
        private ParentHitTestResult HitTestParentControl()
        {
            Form parentForm = _control.FindForm();
            var cursor = parentForm.PointToClient(Control.MousePosition);

            if (TopLeft.Contains(cursor)) return ParentHitTestResult.TopLeft;
            else if (TopRight.Contains(cursor)) return ParentHitTestResult.TopRight;
            else if (BottomLeft.Contains(cursor)) return ParentHitTestResult.BottomLeft;
            else if (BottomRight.Contains(cursor)) return ParentHitTestResult.BottomRight;

            else if (Top.Contains(cursor)) return ParentHitTestResult.Top;
            else if (Left.Contains(cursor)) return ParentHitTestResult.Left;
            else if (Right.Contains(cursor)) return ParentHitTestResult.Right;
            else if (Bottom.Contains(cursor)) return ParentHitTestResult.Bottom;

            return ParentHitTestResult.Null;
        }
    }
}
