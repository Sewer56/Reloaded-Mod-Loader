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
    /// This file handles the font aspect of the form.
    /// </summary>
    public static partial class ApplyTheme
    {
        /// <summary>
        /// Applies the common font style for an individual control.
        /// Changes the font used for the control to the one derived from the theme.
        /// </summary>
        private static void ApplyFonts(Control control)
        {
            // Filter the three text categories.
            if (IsTitleItem(control)) { control.Font = new Font(Theme.Fonts.TitleFont.FontFamily, control.Font.Size, Theme.Fonts.TitleFont.Style, control.Font.Unit); }
            else if (IsCategoryItem(control)) { control.Font = new Font(Theme.Fonts.CategoryFont.FontFamily, control.Font.Size, Theme.Fonts.CategoryFont.Style, control.Font.Unit); }

            else if (IsMainItem(control)) { control.Font = new Font(Theme.Fonts.TextFont.FontFamily, control.Font.Size, Theme.Fonts.TextFont.Style, control.Font.Unit); }
            else if (IsBorderless(control)) { control.Font = new Font(Theme.Fonts.TextFont.FontFamily, control.Font.Size, Theme.Fonts.TextFont.Style, control.Font.Unit); }
            else if (IsBox(control)) { control.Font = new Font(Theme.Fonts.TextFont.FontFamily, control.Font.Size, Theme.Fonts.TextFont.Style, control.Font.Unit); }
        }
    }
}
