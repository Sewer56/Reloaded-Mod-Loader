/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.Drawing;
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
