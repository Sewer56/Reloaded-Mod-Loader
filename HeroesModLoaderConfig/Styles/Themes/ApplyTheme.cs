using SonicHeroes.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Themes
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This method should be called once any specific form is shown.
    /// </summary>
    public static class ApplyTheme
    {
        // Note:
        // Category Bar Text should be named categoryBar_*
        // Regular Text should be named text_*
        // Title Bar Text should be named titleBar_

        /// <summary>
        /// Applies the currently set theming properties to a new windows form.
        /// </summary>
        public static void ApplyCurrentTheme()
        {
            // For each currently initialized Windows Form.
            foreach (Form windowForm in Global.WindowsForms) { ThemeWindowsForm(windowForm); }
        }

        /// <summary>
        /// Themes an individual windows form to the currently set theme.
        /// </summary>
        /// <param name="windowForm">The form whose controls should be themed upon showing.</param>
        public static void ThemeWindowsForm(Form windowForm)
        {
            /// Iterate over each control.
            foreach (Control control in windowForm.Controls)
            {
                // If the control has embedded controls (thus embeds child controls, apply theme to children.
                if (control.Controls.Count != 0) { foreach (Control controlEmbedded in control.Controls) { ApplyFonts(controlEmbedded); } }
                ApplyFonts(control);
            }
        }

        /// <summary>
        /// Applies the common font style for an individual control.
        /// Changes the font used for the control to the one derived from the theme.
        /// </summary>
        private static void ApplyFonts(Control control)
        {
            // Filter the three text categories.
            if (control.Name.StartsWith("categoryBar_")) { control.Font = new Font(Global.Theme.Fonts.CategoryFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
            else if (control.Name.StartsWith("text_")) { control.Font = new Font(Global.Theme.Fonts.TextFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
            else if (control.Name.StartsWith("titleBar_")) { control.Font = new Font(Global.Theme.Fonts.TitleFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
        }

        /// <summary>
        /// Loads all of the theme images from storage onto the relevant base form buttons.
        /// </summary>
        /// <param name="imagesFolder">Dictates the folder where the images are supposed to be loaded from.</param>
        public static void LoadImages(string imagesFolder)
        {
            // If the baseform is instantiated
            if (Global.BaseForm != null)
            {
                // Load the images from HDD.
                Global.BaseForm.categoryBar_About.Image = Image.FromFile(imagesFolder + "\\About-Icon.png");
                Global.BaseForm.categoryBar_Manager.Image = Image.FromFile(imagesFolder + "\\Entry-Icon.png");
                Global.BaseForm.categoryBar_Theme.Image = Image.FromFile(imagesFolder + "\\Paint-Icon.png");
                Global.BaseForm.categoryBar_Input.Image = Image.FromFile(imagesFolder + "\\Controller-Icon.png");
                Global.BaseForm.categoryBar_Mods.Image = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                Global.BaseForm.categoryBar_Games.Image = Image.FromFile(imagesFolder + "\\Main-Icon.png");
            }
        }
    }
}
