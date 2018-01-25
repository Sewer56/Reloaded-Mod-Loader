using HeroesModLoaderConfig.Styles.Animation;
using HeroesModLoaderConfig.Styles.Controls.Animated;
using SonicHeroes.Misc;
using SonicHeroes.Misc.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SonicHeroes.Misc.Config.ThemePropertyParser;

namespace HeroesModLoaderConfig.Styles.Themes
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This method should be called once any specific form is shown.
    /// </summary>
    public static class ApplyTheme
    {
        // Note:
        // Category Bar Items should be named categoryBar_*
        // Regular Items should be named item_*
        // Title Bar Items should be named titleBar_*
        // Buttons used for decoration should be named box_*

        /// <summary>
        /// Applies the currently set theming properties to a new windows form.
        /// </summary>
        public static void ApplyCurrentTheme()
        {
            // For each currently initialized Windows Form.
            foreach (Form windowForm in Global.WindowsForms) { ThemeWindowsForm(windowForm); }

            // If the initial form has been initialized yet.
            if (Global.BaseForm != null)
            {
                // Update the title.
                Global.BaseForm.UpdateTitle();

                // Set the background colours
                Global.BaseForm.panel_CategoryBar.BackColor = Theme.ThemeProperties.CategoryColours.BGColour;
                Global.BaseForm.panel_TitleBar.BackColor = Theme.ThemeProperties.TitleColours.BGColour;
            }
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
                if (control.Controls.Count != 0) { foreach (Control controlEmbedded in control.Controls) { ThemeControl(controlEmbedded); } }

                // Apply the theme.
                ThemeControl(control);
            }

            // Set the BG Colour of the Form
            windowForm.BackColor = Theme.ThemeProperties.MainColours.BGColour;
        }

        /// <summary>
        /// Themes an individual windows forms control.
        /// </summary>
        /// <param name="control"></param>        
        private static void ThemeControl(Control control)
        {
            // Apply the appropriate fonts to the control.
            ApplyFonts(control);

            // Theme the Category, Main and Title Buttons
            if (IsMainItem(control)) { ThemeButton(control, Theme.ThemeProperties.MainColours, Theme.ThemeProperties.MainEnterAnimation, Theme.ThemeProperties.MainLeaveAnimation, true); }
            else if (IsCategoryItem(control)){ ThemeButton(control, Theme.ThemeProperties.CategoryColours, Theme.ThemeProperties.CategoryEnterAnimation, Theme.ThemeProperties.CategoryLeaveAnimation, false); }
            else if (IsTitleItem(control)) { ThemeButton(control, Theme.ThemeProperties.TitleColours, Theme.ThemeProperties.TitleEnterAnimation, Theme.ThemeProperties.TitleLeaveAnimation, false); }
            else if (IsBox(control)) { ThemeButton(control, Theme.ThemeProperties.BoxColours, Theme.ThemeProperties.BoxEnterAnimation, Theme.ThemeProperties.BoxLeaveAnimation, true); }
            else { ThemeButton(control, Theme.ThemeProperties.MainColours, Theme.ThemeProperties.MainEnterAnimation, Theme.ThemeProperties.MainLeaveAnimation, true); }
        }

        /// <summary>
        /// Sets the common properties of an animated main section (not title/category) button.
        /// These properties are common for all main buttons placed anywhere.
        /// Properties that are location dependent, such as borders are set elsewhere.
        /// </summary>
        /// <param name="control">The button control whose properties are to be set.</param>
        /// <param name="ThemeBorder">Adds/Themes the current border if set to true.</param>
        /// <param name="buttonColours">Defines the main button colours of the button in question</param>
        /// <param name="enterAnimation">Defines the mouse enter animation of the button.</param>
        /// <param name="exitAnimation">Defines the mouse exit animation of the buttons.</param>
        private static void ThemeButton(Control control, BarColours buttonColours, ButtonMouseAnimation enterAnimation, ButtonMouseAnimation exitAnimation, bool ThemeBorder)
        {
            // Set the button backcolor and forecolor.
            control.ForeColor = buttonColours.TextColour;
            control.BackColor = buttonColours.ButtonBGColour;

            // Theme the button border.
            if (ThemeBorder) { ThemeButtonBorder(control); }

            // If it is an animated control
            if (control is IAnimatedControl)
            {
                // Cast to AnimatedControl
                IAnimatedControl animatedControl = (IAnimatedControl)control;

                // Set the enter animation.
                animatedControl.AnimProperties.MouseEnterBackColor = enterAnimation.BGTargetColour;
                animatedControl.AnimProperties.MouseEnterForeColor = enterAnimation.FGTargetColour;
                animatedControl.AnimProperties.MouseEnterDuration = enterAnimation.AnimationDuration;
                animatedControl.AnimProperties.MouseEnterFramerate = enterAnimation.AnimationFramerate;
                if (enterAnimation.BlendBGColour) { animatedControl.AnimProperties.MouseEnterOverride = animatedControl.AnimProperties.MouseEnterOverride | Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate; }
                if (enterAnimation.BlendFGColour) { animatedControl.AnimProperties.MouseEnterOverride = animatedControl.AnimProperties.MouseEnterOverride | Animation.AnimOverrides.MouseEnterOverride.ForeColorInterpolate; }

                // Set the exit animation
                animatedControl.AnimProperties.MouseLeaveBackColor = exitAnimation.BGTargetColour;
                animatedControl.AnimProperties.MouseLeaveForeColor = exitAnimation.FGTargetColour;
                animatedControl.AnimProperties.MouseLeaveDuration = exitAnimation.AnimationDuration;
                animatedControl.AnimProperties.MouseLeaveFramerate = exitAnimation.AnimationFramerate;
                if (exitAnimation.BlendBGColour) { animatedControl.AnimProperties.MouseLeaveOverride = animatedControl.AnimProperties.MouseLeaveOverride | Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate; }
                if (exitAnimation.BlendFGColour) { animatedControl.AnimProperties.MouseLeaveOverride = animatedControl.AnimProperties.MouseLeaveOverride | Animation.AnimOverrides.MouseLeaveOverride.ForeColorInterpolate; }
            }

            // If it is a datagridview, set the 
            if (control is DataGridView)
            {
                DataGridView controlDataGridView = (DataGridView)control;
                controlDataGridView.DefaultCellStyle.ForeColor = buttonColours.TextColour;
                controlDataGridView.DefaultCellStyle.BackColor = buttonColours.BGColour;
                controlDataGridView.DefaultCellStyle.SelectionBackColor = enterAnimation.BGTargetColour;
                controlDataGridView.DefaultCellStyle.SelectionForeColor = buttonColours.TextColour;
            }
        }

        /// <summary>
        /// Applies the relevant border style to a windows forms button based off of the loaded
        /// colour/animation configuration file.
        /// </summary>
        /// <param name="control">The button control whose properties are to be set.</param>
        private static void ThemeButtonBorder(Control control)
        {
            // Try to cast the animated button 
            Button button = control as Button;

            // If caster, set border style.
            if (button != null)
            {
                // Set border size.
                button.FlatAppearance.BorderSize = Theme.ThemeProperties.ButtonBorderProperties.BorderWidth;
                button.FlatAppearance.BorderColor = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;
            }
        }

        /// <summary>
        /// Applies the common font style for an individual control.
        /// Changes the font used for the control to the one derived from the theme.
        /// </summary>
        private static void ApplyFonts(Control control)
        {
            // Filter the three text categories.
            if (IsCategoryItem(control)) { control.Font = new Font(Theme.Fonts.CategoryFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
            else if (IsMainItem(control)) { control.Font = new Font(Theme.Fonts.TextFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
            else if (IsTitleItem(control)) { control.Font = new Font(Theme.Fonts.TitleFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
            else if (IsBox(control)) { control.Font = new Font(Theme.Fonts.TextFont.FontFamily, control.Font.Size, control.Font.Style, control.Font.Unit); }
        }

        /// <summary>
        /// Returns true if the specified passed in control is part of the main form
        /// and not the category/title bar.
        /// </summary>
        public static bool IsMainItem(Control control) { return control.Name.StartsWith("item_") ? true : false; }

        /// <summary>
        /// Returns true if the specified passed in control is a box used for decoration.
        /// (That is a button with no purposeful functionality)
        /// </summary>
        public static bool IsBox(Control control) { return control.Name.StartsWith("box_") ? true : false; }

        /// <summary>
        /// Returns true if the specified passed in control is part of the main form
        /// and not the category/title bar.
        /// </summary>
        public static bool IsCategoryItem(Control control) { return control.Name.StartsWith("categoryBar_") ? true : false; }

        /// <summary>
        /// Returns true if the specified passed in control is part of the main form
        /// and not the category/title bar.
        /// </summary>
        public static bool IsTitleItem(Control control) { return control.Name.StartsWith("titleBar_") ? true : false; }

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

        /// <summary>
        /// Loads the theme configuration including the Windows Control Colours and Animations
        /// </summary>
        /// <param name="themeDirectory">Dictates the name of the directory that contains the theme with the colour/animation configuration. e.g. "Default" for default theme.</param>
        public static void LoadProperties(string themeDirectory)
        {
            // If the baseform is instantiated
            ThemePropertyParser themeColourParser = new ThemePropertyParser();
            Theme.ThemeProperties = themeColourParser.ParseConfig(themeDirectory);
        }
    }
}
