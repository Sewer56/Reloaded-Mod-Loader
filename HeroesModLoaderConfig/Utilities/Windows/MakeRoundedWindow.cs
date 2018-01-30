using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Utilities.Windows
{
    /// <summary>
    /// Allows for setting the region of a windows form such that the form gains rounded edges.
    /// </summary>
    class MakeRoundedWindow
    {
        /// <summary>
        /// The CreateRoundRectRgn function creates a rectangular region with rounded corners.
        /// </summary>
        /// <param name="nLeftRect">Specifies the x-coordinate of the upper-left corner of the region in device units.</param>
        /// <param name="nTopRect">Specifies the y-coordinate of the upper-left corner of the region in device units.</param>
        /// <param name="nRightRect">Specifies the x-coordinate of the lower-right corner of the region in device units.</param>
        /// <param name="nBottomRect">Specifies the y-coordinate of the lower-right corner of the region in device units.</param>
        /// <param name="nWidthEllipse">Specifies the width of the ellipse used to create the rounded corners in device units.</param>
        /// <param name="nHeightEllipse">Specifies the height of the ellipse used to create the rounded corners in device units.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// Rounds a Windows Form window.
        /// </summary>
        /// <param name="winForm">Windows form window</param>
        /// <param name="heightOfEllipse">Specifies the width of the ellipse used to create the rounded corners in device units.</param>
        /// <param name="widthOfEllipse">Specifies the height of the ellipse used to create the rounded corners in device units.</param>
        public static void RoundWindow(Form winForm, int widthOfEllipse, int heightOfEllipse)
        {
            winForm.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, winForm.Width, winForm.Height, widthOfEllipse, heightOfEllipse));
        }
    }
}
