using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SonicHeroes.Native.WinAPI;

namespace SonicHeroes.Native
{
    /// <summary>
    /// Provides various classes for obtaining information about various windows that are currently present.
    /// </summary>
    static class Windows
    {
        /// <summary>
        /// Returns the coordinates of the edges of a specific window relative to the desktop the window is presented on.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the window rectangle should be obtained.</param>
        /// <returns></returns>
        public static WinAPIRectangle GetWindowRectangle(IntPtr windowHandle)
        {
            // Stores the rectangle which defines Sonic Heroes' game window.
            WinAPIRectangle gameWindowRectangle = new WinAPIRectangle();

            // Obtains the coordinates of the edges of the window.
            WinAPI.Windows.GetWindowRect(windowHandle, out gameWindowRectangle);

            // Return
            return gameWindowRectangle;
        }

        /// <summary>
        /// Returns the coordinates of the edges of a specific window in terms of 
        /// X from the left and Y from the top of the window. 
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns></returns>
        public static WinAPIRectangle GetClientAreaRectangle(IntPtr windowHandle)
        {
            // Stores the rectangle which defines Sonic Heroes' game window.
            WinAPIRectangle clientAreaRectangle = new WinAPIRectangle();

            // Obtains the coordinates of the edges of the window.
            WinAPI.Windows.GetClientRect(windowHandle, out clientAreaRectangle);

            // Return
            return clientAreaRectangle;
        }

        /// <summary>
        /// Returns the border width in terms of X and Y for a window.
        /// </summary>
        /// <returns></returns>
        public static Point GetBorderWidth(WinAPIRectangle gameWindowRectangle, WinAPIRectangle gameClientRectangle)
        {
            // Stores the size of the border vertically and horizontally.
            Point totalBorderSize = new Point();

            // Calculate the width and height of the window.
            int windowWidth = gameWindowRectangle.rightBorder - gameWindowRectangle.leftBorder;
            int windowHeight = gameWindowRectangle.bottomBorder - gameWindowRectangle.topBorder;

            // Remove the client area width/height to leave only the borders.
            totalBorderSize.X = windowWidth - gameClientRectangle.rightBorder;
            totalBorderSize.Y = windowHeight - gameClientRectangle.bottomBorder;

            // Return the borders.
            return totalBorderSize;
        }

        /// <summary>
        /// Retrieves the client area size of a window. This is a slight alias for Get_Game_ClientArea_Rectangle.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns>Width as X and Height as Y of the window client area requested.</returns>
        public static Point GetWindowClientSize(IntPtr windowHandle)
        {
            // Get Window Client-Area
            WinAPIRectangle windowClientArea = GetClientAreaRectangle(windowHandle);

            // Return window internal size.
            return new Point(windowClientArea.rightBorder, windowClientArea.bottomBorder);
        }


        /// <summary>
        /// Retrieves the client area size of a window. This is a slight alias for Get_Game_ClientArea_Rectangle.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns>Width as X and Height as Y of the window client area requested.</returns>
        public static Point GetWindowSize(IntPtr windowHandle)
        {
            // Get Window Client-Area
            WinAPIRectangle windowSizeRectangle = GetWindowRectangle(windowHandle);

            // Define height and width
            int windowWidth = windowSizeRectangle.rightBorder - windowSizeRectangle.leftBorder;
            int windowHeight = windowSizeRectangle.bottomBorder - windowSizeRectangle.topBorder;

            // Return window internal size.
            return new Point(windowSizeRectangle.rightBorder, windowSizeRectangle.bottomBorder);
        }

    }
}
