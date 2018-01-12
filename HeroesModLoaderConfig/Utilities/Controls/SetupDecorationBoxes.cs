using HeroesModLoaderConfig.Styles.Controls;
using HeroesModLoaderConfig.Styles.Controls.Interfaces;
using HeroesModLoaderConfig.Styles.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace HeroesModLoaderConfig.Utilities.Controls
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
