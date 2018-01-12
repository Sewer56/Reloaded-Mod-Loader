using SonicHeroes.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Controls.Enhanced
{
    /// <summary>
    /// Provides a customized implementation of a listview that removes the vertical and horizontal scrollbars from the 
    /// listview control itself. 
    /// </summary>
    class EnhancedListview : ListView
    {
        /// <summary>
        /// Overrides the WndProc function that handles messages sent to the ListView
        /// control in question. It removes the scrollbars from the listview when the listview
        /// in the case that the control may want to place them.
        /// We accomodate scrolling ourselves by using the arrow keys and scroll wheel.
        /// </summary>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                // WM_NCCALCSIZE | Message that calculates the size of the window.
                case 0x83:
                    // Obtain Initial Style
                    long windowStyle = (long)WinAPI.WindowStyles.GetWindowLongPtr(this.Handle, WinAPI.WindowStyles.Constants.GWL_STYLE); 

                    // If the initial style for the Window contains the vertical scrollbar, remove it from the window style.
                    if ((windowStyle & WinAPI.WindowStyles.Constants.WS_HSCROLL) == WinAPI.WindowStyles.Constants.WS_HSCROLL)
                    { windowStyle = windowStyle & ~WinAPI.WindowStyles.Constants.WS_HSCROLL; }

                    // Repeat for horizontal scrollbar if it is contained in the window style.
                    if ((windowStyle & WinAPI.WindowStyles.Constants.WS_VSCROLL) == WinAPI.WindowStyles.Constants.WS_VSCROLL)
                    { windowStyle = windowStyle & ~WinAPI.WindowStyles.Constants.WS_VSCROLL; }

                    // Write the initial window style.
                    WinAPI.WindowStyles.SetWindowLongPtr(new HandleRef(this, this.Handle), WinAPI.WindowStyles.Constants.GWL_STYLE, (IntPtr)windowStyle);

                    // Send the message to the base function, for potential painting purposes.
                    base.WndProc(ref message);
                    break;

                // Ignore WM_STYLECHANGED (No Permission to write, this is a Win32 wrapped control)
                case 0x7D:
                    break;

                // Ignore WM_REFLECT | WM_NOTIFY
                case 0x204e:
                    break;

                // No modification.
                default:
                    base.WndProc(ref message); 
                    break;
            }
        }

        public EnhancedListview()
        {
            this.Scrollable = false;
        }
    }
}
