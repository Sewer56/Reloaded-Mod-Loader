﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Controls.Interfaces
{
    /// <summary>
    /// Defines a control which supports the use of borders.
    /// Allows for the drawing of borders around controls.
    /// </summary>
    interface IBorderedControl
    {
        // Border Colours
        Color LeftBorderColour { get; set; }
        Color TopBorderColour { get; set; }
        Color RightBorderColour { get; set; }
        Color BottomBorderColour { get; set; }

        // Border Styles
        ButtonBorderStyle LeftBorderStyle { get; set; }
        ButtonBorderStyle RightBorderStyle { get; set; }
        ButtonBorderStyle TopBorderStyle { get; set; }
        ButtonBorderStyle BottomBorderStyle { get; set; }

        // Border Widths
        int LeftBorderWidth { get; set; }
        int RightBorderWidth { get; set; }
        int TopBorderWidth { get; set; }
        int BottomBorderWidth { get; set; }
    }
}
