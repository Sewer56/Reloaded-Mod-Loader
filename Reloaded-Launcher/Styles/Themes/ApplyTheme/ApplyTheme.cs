using ReloadedLauncher.Styles.Animation;
using ReloadedLauncher.Styles.Controls.Interfaces;
using Reloaded.Misc.Config;
using System;
using System.Drawing;
using ReloadedLauncher.Utilities.Windows;
using System.Windows.Forms;
using static Reloaded.Misc.Config.ThemePropertyParser;
using ReloadedLauncher.Styles.Controls.Enhanced;
using ReloadedLauncher.Styles.Controls.Custom;

namespace ReloadedLauncher.Styles.Themes
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This method should be called once any specific form is shown.
    /// </summary>
    public static partial class ApplyTheme
    {
        // Note:
        // Category Bar Items should be named categoryBar_*
        // Regular Items should be named item_*
        // Title Bar Items should be named titleBar_*
        // Buttons used for decoration should be named box_*
        // Buttons (Alternative style) should be named borderless_*

        /// <summary>
        /// Applies the currently set theming properties to a new windows form.
        /// </summary>
        public static void ApplyCurrentTheme()
        {
            // For each currently initialized Windows Form.
            foreach (Form windowForm in Global.WindowsForms) { ThemeWindowsForm(windowForm); }

            // If the initial form has been initialized.
            if (Global.BaseForm != null)
            {
                // Update the title.
                Global.BaseForm.UpdateTitle("");

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
            else if (IsBorderless(control)) { ThemeButton(control, Theme.ThemeProperties.BorderlessColours, Theme.ThemeProperties.BorderlessEnterAnimation, Theme.ThemeProperties.BorderlessLeaveAnimation, false); }
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
            // Set the control backcolor and forecolor.
            control.ForeColor = buttonColours.TextColour;
            control.BackColor = buttonColours.ButtonBGColour;

            // If it is an animated control
            #region Animated Controls
            if (control is IAnimatedControl)
            {
                // Cast to AnimatedControl
                IAnimatedControl animatedControl = (IAnimatedControl)control;

                // Reset animations.
                animatedControl.AnimProperties.MouseEnterOverride = AnimOverrides.MouseEnterOverride.None;
                animatedControl.AnimProperties.MouseLeaveOverride = AnimOverrides.MouseLeaveOverride.None;

                // Set the enter animation.
                animatedControl.AnimProperties.MouseEnterBackColor = enterAnimation.BGTargetColour;
                animatedControl.AnimProperties.MouseEnterForeColor = enterAnimation.FGTargetColour;
                animatedControl.AnimProperties.MouseEnterDuration = enterAnimation.AnimationDuration;
                if (enterAnimation.BlendBGColour) { animatedControl.AnimProperties.MouseEnterOverride = animatedControl.AnimProperties.MouseEnterOverride | Animation.AnimOverrides.MouseEnterOverride.BackColorInterpolate; }
                if (enterAnimation.BlendFGColour) { animatedControl.AnimProperties.MouseEnterOverride = animatedControl.AnimProperties.MouseEnterOverride | Animation.AnimOverrides.MouseEnterOverride.ForeColorInterpolate; }

                // Set the exit animation
                animatedControl.AnimProperties.MouseLeaveBackColor = exitAnimation.BGTargetColour;
                animatedControl.AnimProperties.MouseLeaveForeColor = exitAnimation.FGTargetColour;
                animatedControl.AnimProperties.MouseLeaveDuration = exitAnimation.AnimationDuration;
                
                if (exitAnimation.BlendBGColour) { animatedControl.AnimProperties.MouseLeaveOverride = animatedControl.AnimProperties.MouseLeaveOverride | Animation.AnimOverrides.MouseLeaveOverride.BackColorInterpolate; }
                if (exitAnimation.BlendFGColour) { animatedControl.AnimProperties.MouseLeaveOverride = animatedControl.AnimProperties.MouseLeaveOverride | Animation.AnimOverrides.MouseLeaveOverride.ForeColorInterpolate; }

                // Overrate Framerates if not specified
                if (animatedControl.AnimProperties.MouseEnterFramerate == 0) { animatedControl.AnimProperties.MouseEnterFramerate = enterAnimation.AnimationFramerate; }
                if (animatedControl.AnimProperties.MouseLeaveFramerate == 0) { animatedControl.AnimProperties.MouseLeaveFramerate = exitAnimation.AnimationFramerate; }

                // If it is a bordered, control, override.
                if (control is IBorderedControl)
                {
                    // Cast to AnimatedControl
                    IBorderedControl borderedControl = (IBorderedControl)control;
                    animatedControl.AnimProperties.MouseLeaveForeColor = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;
                }
            }
            #endregion Animated Controls

            // If it is a datagridview, set the 
            #region DataGridView
            if (control is DataGridView)
            {
                DataGridView controlDataGridView = (DataGridView)control;

                // Cells
                controlDataGridView.DefaultCellStyle.ForeColor = buttonColours.TextColour;
                controlDataGridView.DefaultCellStyle.BackColor = buttonColours.BGColour;
                controlDataGridView.DefaultCellStyle.SelectionBackColor = enterAnimation.BGTargetColour;
                controlDataGridView.DefaultCellStyle.SelectionForeColor = buttonColours.TextColour;

                // Columns
                controlDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = buttonColours.TextColour;
                controlDataGridView.ColumnHeadersDefaultCellStyle.BackColor = buttonColours.BGColour;
                controlDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = enterAnimation.BGTargetColour;
                controlDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = buttonColours.TextColour;

                // Fonts
                controlDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font
                (
                    Theme.Fonts.TextFont.FontFamily,
                    controlDataGridView.ColumnHeadersDefaultCellStyle.Font.Size, 
                    Theme.Fonts.TextFont.Style,
                    controlDataGridView.ColumnHeadersDefaultCellStyle.Font.Unit
                );

                controlDataGridView.BackgroundColor = buttonColours.ButtonBGColour;
                
                // Set row properties
                foreach (DataGridViewRow row in controlDataGridView.Rows)
                {
                    row.DefaultCellStyle.ForeColor = buttonColours.TextColour;
                    row.DefaultCellStyle.BackColor = buttonColours.BGColour;
                    row.DefaultCellStyle.SelectionBackColor = enterAnimation.BGTargetColour;
                    row.DefaultCellStyle.SelectionForeColor = buttonColours.TextColour;
                }
            }
            #endregion DataGridView

            // If it is a enhanced combobox
            #region EnhancedComboBox
            if (control is EnhancedComboBox)
            {
                // Enhanced ComboBox
                EnhancedComboBox enhancedComboBox = (EnhancedComboBox)control;

                enhancedComboBox.BackColor = buttonColours.ButtonBGColour;
                enhancedComboBox.DropDownButtonColour = buttonColours.ButtonBGColour;
                enhancedComboBox.DropDownArrowColour = buttonColours.TextColour;
                enhancedComboBox.HighlightColor = enterAnimation.BGTargetColour;
                enhancedComboBox.ForeColor = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;
            }
            #endregion EnhancedComboBox

            // Analog stick indicator
            #region Analog Stick Indicator
            if (control is CustomAnalogStickIndicator)
            {
                // Cast Bordered Control
                CustomAnalogStickIndicator stickIndicator = (CustomAnalogStickIndicator)control;

                // Set Indicator Colour.
                stickIndicator.IndicatorColour = buttonColours.TextColour;
            }
            #endregion Analog Stick Indicator

            // If the box has a border.
            #region Vertical Progress Bar
            if (control is CustomVerticalProgressBar)
            {
                // Cast Bordered Control
                CustomVerticalProgressBar verticalProgressBar = (CustomVerticalProgressBar)control;

                // Set Indicator Colour.
                verticalProgressBar.ProgressColour = buttonColours.TextColour;
            }
            #endregion Vertical Progress Bar

            // If the box has a border.
            #region IBordered Control (Bordered Controls)
            if (control is IBorderedControl)
            {
                // Cast Bordered Control
                IBorderedControl borderedControl = (IBorderedControl)control;

                borderedControl.BottomBorderColour = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;
                borderedControl.LeftBorderColour = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;
                borderedControl.TopBorderColour = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;
                borderedControl.RightBorderColour = Theme.ThemeProperties.ButtonBorderProperties.BorderColour;

                borderedControl.LeftBorderWidth = Theme.ThemeProperties.ButtonBorderProperties.BorderWidth;
                borderedControl.RightBorderWidth = Theme.ThemeProperties.ButtonBorderProperties.BorderWidth;
                borderedControl.TopBorderWidth = Theme.ThemeProperties.ButtonBorderProperties.BorderWidth;
                borderedControl.BottomBorderWidth = Theme.ThemeProperties.ButtonBorderProperties.BorderWidth;

                // Match forecolor to borders if it's a textbox.
                EnhancedTextbox textbox = control as EnhancedTextbox;
                if (textbox != null) { textbox.ForeColor = Theme.ThemeProperties.ButtonBorderProperties.BorderColour; }
            }
            #endregion IBordered Control (Bordered Controls)

            // If the control is a box, set BG colour of children.
            #region Decoration Boxes
            if (control is IDecorationBox)
            {
                foreach (Control childControl in control.Controls)
                {
                    childControl.BackColor = control.BackColor;
                }
            }
            #endregion Decoration Boxes

            // Theme the control border.
            if (ThemeBorder) { ThemeButtonBorder(control); }
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
        /// Returns true if the specified passed in control is an alternative style button (borderless)
        /// </summary>
        public static bool IsBorderless(Control control) { return control.Name.StartsWith("borderless_") ? true : false; }

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
