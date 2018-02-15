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

using ReloadedLauncher.Styles.Controls.Interfaces;
using ReloadedLauncher.Styles.Themes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ReloadedLauncher.Utilities.Controls
{
    /// <summary>
    /// For Windows Forms, Finds all of the Children of Decoration boxes marked as box_* in their names
    /// and brings any other controls that are in their region space under their control.
    /// </summary>
    public static class SetupDecorationBoxes
    {
        /// <summary>
        /// Detects the decoration boxes of a windows form and automatically appends any other
        /// controls which are within the region space of the decoration box to the box.
        /// </summary>
        /// <param name="winForm">Windows form whose controls are to be checked.</param>
        public static void FindDecorationControls(Form winForm)
        {
            // Get the list of all button contorls
            List<Control> allControls = new List<Control>();

            // Find all button controls
            // We need to first grab all controls into a list ourselves, as adding
            // a control to be a child of X will cause it to no longer to be a child of the form,
            // causing otherwise the foreach loop to end early as winForm.Controls changes.
            foreach (Control control in winForm.Controls) { allControls.Add(control); }

            // Find every Box Control
            foreach (Control control in allControls)
            {
                // If the control is of box type.
                if ((control is IDecorationBox) && ApplyTheme.IsBox(control))
                {
                    // Find all controls which overlap their location with the region of the box.
                    foreach (Control childControl in allControls)
                    {
                        if
                        (
                            // Check if the control fits in the X Axis Region
                            ((childControl.Location.X > control.Location.X) && (childControl.Location.X < control.Location.X + control.Size.Width)) &&

                            // Check if the control fits in the Y Axis Region
                            ((childControl.Location.Y > control.Location.Y) && (childControl.Location.Y < control.Location.Y + control.Size.Height))
                        )
                        {
                            // Cast to decoration box, check if to capture children
                            IDecorationBox decorationBox = (IDecorationBox)control;

                            // Ignore the mouse if it is an EnhancedLabel
                            if (childControl is IControlIgnorable) { IControlIgnorable label = (IControlIgnorable)childControl; label.IgnoreMouse = true; }

                            // Add as child if the box is set to capture children.
                            if (decorationBox.CaptureChildren)
                            {
                                // Add the control as child
                                control.Controls.Add(childControl);

                                // Update BackColor with parent
                                control.BackColorChanged += Control_BackColorChanged;

                                // Set location relative to parent
                                SetLocationRelative.SetRelativeLocation(control, childControl);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Paints the background colour of any of the children decoration boxes
        /// to the colour of the decoration box background (WinForms does not support transparency
        /// properly, this is a workaround).
        /// </summary>
        private static void Control_BackColorChanged(object sender, EventArgs e)
        {
            // Cast the sender as the control.
            Control locationBox = (Control)sender;

            // For each child, set the backcolour.
            foreach (Control childControl in locationBox.Controls) { childControl.BackColor = locationBox.BackColor; }
        }
    }
}
