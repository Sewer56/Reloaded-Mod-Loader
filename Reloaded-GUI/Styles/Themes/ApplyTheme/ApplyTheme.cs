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
using Reloaded.IO.Config;
using Reloaded_GUI.Styles.Animation;
using Reloaded_GUI.Styles.Controls.Animated;
using Reloaded_GUI.Styles.Controls.Custom;
using Reloaded_GUI.Styles.Controls.Enhanced;
using Reloaded_GUI.Styles.Controls.Interfaces;
using Reloaded_GUI.Utilities.Windows;

namespace Reloaded_GUI.Styles.Themes.ApplyTheme
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
        /// Returns true if the specified passed in control is part of the main form
        /// and not the category/title bar.
        /// </summary>
        public static bool IsMainItem(Control control) { return control.Name.StartsWith("item_"); }

        /// <summary>
        /// Returns true if the specified passed in control is a box used for decoration.
        /// (That is a button with no purposeful functionality)
        /// </summary>
        public static bool IsBox(Control control) { return control.Name.StartsWith("box_"); }

        /// <summary>
        /// Returns true if the specified passed in control is an alternative style button (borderless)
        /// </summary>
        public static bool IsBorderless(Control control) { return control.Name.StartsWith("borderless_"); }

        /// <summary>
        /// Returns true if the specified passed in control is part of the main form
        /// and not the category/title bar.
        /// </summary>
        public static bool IsCategoryItem(Control control) { return control.Name.StartsWith("categoryBar_"); }

        /// <summary>
        /// Returns true if the specified passed in control is part of the main form
        /// and not the category/title bar.
        /// </summary>
        public static bool IsTitleItem(Control control) { return control.Name.StartsWith("titleBar_"); }

        /// <summary>
        /// Loads the theme configuration including the Windows Control Colours and Animations.
        /// </summary>
        /// <param name="themeDirectory">Dictates the name of the directory that contains the theme with the colour/animation configuration. e.g. "Default" for default theme.</param>
        public static void LoadProperties(string themeDirectory)
        {
            // If the baseform is instantiated
            Theme.ThemeProperties = ThemeProperties.Theme.ParseConfig(themeDirectory);
        }

        /// <summary>
        /// Applies the currently set theming properties to a new windows form.
        /// </summary>
        public static void ApplyCurrentTheme()
        {
            // For each currently initialized Windows Form.
            foreach (Form windowForm in Bindings.WindowsForms)
                ThemeWindowsForm(windowForm);
        }

        /// <summary>
        /// Themes an individual windows form to the currently set theme.
        /// </summary>
        /// <param name="windowForm">The form whose controls should be themed upon showing.</param>
        public static void ThemeWindowsForm(Form windowForm)
        {
            // Iterate over each control.
            foreach (Control control in windowForm.Controls)
            {
                // If the control has embedded controls (thus embeds child controls, apply theme to children.
                if (control.Controls.Count != 0)
                    foreach (Control controlEmbedded in control.Controls)
                        ThemeControl(controlEmbedded);

                // Apply the theme.
                ThemeControl(control);
            }

            // Set the BG Colour of the Form
            windowForm.BackColor = Theme.ThemeProperties.MainColours.BgColour;
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
            if (IsMainItem(control))
                ThemeButton(control, Theme.ThemeProperties.MainColours, Theme.ThemeProperties.MainEnterAnimation, Theme.ThemeProperties.MainLeaveAnimation, true);

            else if (IsCategoryItem(control))
                ThemeButton(control, Theme.ThemeProperties.CategoryColours, Theme.ThemeProperties.CategoryEnterAnimation, Theme.ThemeProperties.CategoryLeaveAnimation, false);
            
            else if (IsBox(control))
                ThemeButton(control, Theme.ThemeProperties.BoxColours, Theme.ThemeProperties.BoxEnterAnimation, Theme.ThemeProperties.BoxLeaveAnimation, true);

            else if (IsBorderless(control))
                ThemeButton(control, Theme.ThemeProperties.BorderlessColours, Theme.ThemeProperties.BorderlessEnterAnimation, Theme.ThemeProperties.BorderlessLeaveAnimation, false);

            else if (IsTitleItem(control))
            {
                ThemeButton(control, Theme.ThemeProperties.TitleColours, Theme.ThemeProperties.TitleEnterAnimation, Theme.ThemeProperties.TitleLeaveAnimation, false);

                // Automatic Dragging!
                control.MouseDown += (sender, args) => MoveWindow.MoveTheWindow(control.FindForm().Handle);
            }

            else
                ThemeButton(control, Theme.ThemeProperties.MainColours, Theme.ThemeProperties.MainEnterAnimation, Theme.ThemeProperties.MainLeaveAnimation, true);
        }

        /// <summary>
        /// Sets the common properties of an animated main section (not title/category) button.
        /// These properties are common for all main buttons placed anywhere.
        /// Properties that are location dependent, such as borders are set elsewhere.
        /// </summary>
        /// <param name="control">The button control whose properties are to be set.</param>
        /// <param name="themeBorder">Adds/Themes the current border if set to true.</param>
        /// <param name="buttonColours">Defines the main button colours of the button in question</param>
        /// <param name="enterAnimation">Defines the mouse enter animation of the button.</param>
        /// <param name="exitAnimation">Defines the mouse exit animation of the buttons.</param>
        private static void ThemeButton(Control control, ThemeProperties.BarColours buttonColours, ThemeProperties.ButtonMouseAnimation enterAnimation, ThemeProperties.ButtonMouseAnimation exitAnimation, bool themeBorder)
        {
            // Set the control backcolor and forecolor.
            control.ForeColor = buttonColours.TextColour;
            control.BackColor = buttonColours.ButtonBgColour;

            // If it is an animated control
            #region Animated Controls
            if (control is IAnimatedControl animatedControl)
            {
                // Reset animations.
                animatedControl.AnimProperties.MouseEnterOverride = AnimOverrides.MouseEnterOverride.None;
                animatedControl.AnimProperties.MouseLeaveOverride = AnimOverrides.MouseLeaveOverride.None;

                // Set the enter animation.
                animatedControl.AnimProperties.MouseEnterBackColor = enterAnimation.BgTargetColour;
                animatedControl.AnimProperties.MouseEnterForeColor = enterAnimation.FgTargetColour;
                animatedControl.AnimProperties.MouseEnterDuration = enterAnimation.AnimationDuration;
                if (enterAnimation.BlendBgColour) animatedControl.AnimProperties.MouseEnterOverride = animatedControl.AnimProperties.MouseEnterOverride | AnimOverrides.MouseEnterOverride.BackColorInterpolate;
                if (enterAnimation.BlendFgColour) animatedControl.AnimProperties.MouseEnterOverride = animatedControl.AnimProperties.MouseEnterOverride | AnimOverrides.MouseEnterOverride.ForeColorInterpolate;

                // Set the exit animation
                animatedControl.AnimProperties.MouseLeaveBackColor = exitAnimation.BgTargetColour;
                animatedControl.AnimProperties.MouseLeaveForeColor = exitAnimation.FgTargetColour;
                animatedControl.AnimProperties.MouseLeaveDuration = exitAnimation.AnimationDuration;
                
                if (exitAnimation.BlendBgColour) animatedControl.AnimProperties.MouseLeaveOverride = animatedControl.AnimProperties.MouseLeaveOverride | AnimOverrides.MouseLeaveOverride.BackColorInterpolate;
                if (exitAnimation.BlendFgColour) animatedControl.AnimProperties.MouseLeaveOverride = animatedControl.AnimProperties.MouseLeaveOverride | AnimOverrides.MouseLeaveOverride.ForeColorInterpolate;

                // Overrate Framerates if not specified
                // ReSharper disable once CompareOfFloatsByEqualityOperator

                if (animatedControl.AnimProperties.MouseEnterFramerate == 0) animatedControl.AnimProperties.MouseEnterFramerate = enterAnimation.AnimationFramerate;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (animatedControl.AnimProperties.MouseLeaveFramerate == 0) animatedControl.AnimProperties.MouseLeaveFramerate = exitAnimation.AnimationFramerate;

                // If it is a bordered, control, override.
                if (animatedControl is IBorderedControl)
                {
                    animatedControl.AnimProperties.MouseLeaveForeColor = Theme.ThemeProperties.BorderProperties.BorderColour;
                }
            }
            #endregion Animated Controls

            // If it is a datagridview, set the 
            #region DataGridView
            if (control is DataGridView controlDataGridView)
            {
                // Cells
                controlDataGridView.DefaultCellStyle.ForeColor = buttonColours.TextColour;
                controlDataGridView.DefaultCellStyle.BackColor = buttonColours.BgColour;
                controlDataGridView.DefaultCellStyle.SelectionForeColor = buttonColours.TextColour;

                // Set Highlight Colour to Animation colour if defined.
                if (enterAnimation.BlendBgColour)
                {
                    controlDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = enterAnimation.BgTargetColour;
                    controlDataGridView.DefaultCellStyle.SelectionBackColor = enterAnimation.BgTargetColour;
                }
                else
                {
                    controlDataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = buttonColours.ButtonBgColour;
                    controlDataGridView.DefaultCellStyle.SelectionBackColor = buttonColours.ButtonBgColour;
                }
                
                // Columns
                controlDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = buttonColours.TextColour;
                controlDataGridView.ColumnHeadersDefaultCellStyle.BackColor = buttonColours.BgColour;
                controlDataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor = buttonColours.TextColour;

                // Fonts
                controlDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font
                (
                    TextFont.FontFamily,
                    controlDataGridView.ColumnHeadersDefaultCellStyle.Font.Size, 
                    TextFont.Style,
                    controlDataGridView.ColumnHeadersDefaultCellStyle.Font.Unit
                );

                controlDataGridView.BackgroundColor = buttonColours.ButtonBgColour;
                
                // Set row properties
                foreach (DataGridViewRow row in controlDataGridView.Rows)
                {
                    row.DefaultCellStyle.ForeColor = buttonColours.TextColour;
                    row.DefaultCellStyle.BackColor = buttonColours.BgColour;
                    row.DefaultCellStyle.SelectionBackColor = enterAnimation.BlendBgColour ? enterAnimation.BgTargetColour : buttonColours.ButtonBgColour;
                    row.DefaultCellStyle.SelectionForeColor = buttonColours.TextColour;
                }
            }
            #endregion DataGridView

            // If it is a enhanced combobox
            #region EnhancedComboBox
            if (control is EnhancedComboBox enhancedComboBox)
            {
                enhancedComboBox.BackColor = buttonColours.ButtonBgColour;
                enhancedComboBox.DropDownButtonColour = buttonColours.ButtonBgColour;
                enhancedComboBox.DropDownArrowColour = buttonColours.TextColour;
                enhancedComboBox.HighlightColor = buttonColours.ButtonBgColour;
                enhancedComboBox.ForeColor = Theme.ThemeProperties.BorderProperties.BorderColour;
            }
            #endregion EnhancedComboBox

            // Analog stick indicator
            #region Analog Stick Indicator
            if (control is AnalogStickIndicator stickIndicator)
            {
                // Set Indicator Colour.
                stickIndicator.IndicatorColour = buttonColours.TextColour;
            }
            #endregion Analog Stick Indicator

            // If the box has a border.
            #region Vertical Progress Bar
            if (control is VerticalProgressBar verticalProgressBar)
            {
                // Set Indicator Colour.
                verticalProgressBar.ProgressColour = buttonColours.TextColour;
            }
            #endregion Vertical Progress Bar

            // If the box has a border.
            #region IBordered Control (Bordered Controls)
            if (control is IBorderedControl borderedControl)
            {
                borderedControl.BottomBorderColour = Theme.ThemeProperties.BorderProperties.BorderColour;
                borderedControl.LeftBorderColour = Theme.ThemeProperties.BorderProperties.BorderColour;
                borderedControl.TopBorderColour = Theme.ThemeProperties.BorderProperties.BorderColour;
                borderedControl.RightBorderColour = Theme.ThemeProperties.BorderProperties.BorderColour;

                borderedControl.LeftBorderWidth = Theme.ThemeProperties.BorderProperties.BorderWidth;
                borderedControl.RightBorderWidth = Theme.ThemeProperties.BorderProperties.BorderWidth;
                borderedControl.TopBorderWidth = Theme.ThemeProperties.BorderProperties.BorderWidth;
                borderedControl.BottomBorderWidth = Theme.ThemeProperties.BorderProperties.BorderWidth;

                // Match forecolor to borders if it's a textbox.
                if (borderedControl is EnhancedTextbox textbox)
                    textbox.ForeColor = Theme.ThemeProperties.BorderProperties.BorderColour;
            }
            #endregion IBordered Control (Bordered Controls)

            // Custom Checkboxes
            #region Animated Button Press Indicators
            if (control is AnimatedButtonPressIndicator animatedButtonPressIndicator)
            {
                animatedButtonPressIndicator.ForeColor = Theme.ThemeProperties.BorderProperties.BorderColour;
            }
            #endregion Animated Button Press Indicators

            // If the control is a box, set BG colour of children.
            #region Decoration Boxes
            if (control is IDecorationBox)
                foreach (Control childControl in control.Controls) childControl.BackColor = control.BackColor;

            #endregion Decoration Boxes

            // Theme the control border.
            if (themeBorder) ThemeButtonBorder(control);
        }

        /// <summary>
        /// Applies the relevant border style to a windows forms button based off of the loaded
        /// colour/animation configuration file.
        /// </summary>
        /// <param name="control">The button control whose properties are to be set.</param>
        private static void ThemeButtonBorder(Control control)
        {
            // If caster, set border style.
            if (control is Button button)
            {
                // Set border size.
                button.FlatAppearance.BorderSize = Theme.ThemeProperties.BorderProperties.BorderWidth;
                button.FlatAppearance.BorderColor = Theme.ThemeProperties.BorderProperties.BorderColour;
            }
        }
    }
}
