using ReloadedLauncher.Styles.Misc;
using ReloadedLauncher.Styles.Themes;
using ReloadedLauncher.Utilities.Controls;
using Reloaded.Misc.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using Reloaded.Input;
using static Reloaded.Input.ControllerCommon;

namespace ReloadedLauncher.Windows.Children
{
    public partial class Input_Screen : Form
    {
        /// <summary>
        /// Specifies a controller manager object,
        /// used for the management of game controllers.
        /// </summary>
        ControllerManager controllerManager;

        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="MDIParent">The MDI Parent form, an instance of Base.cs</param>
        public Input_Screen(Form MDIParent)
        {
            // Standard Winforms Initialization
            InitializeComponent();

            // Set the MDI parent
            MdiParent = MDIParent;

            // Add to the window list.
            Global.WindowsForms.Add(this);

            // Add Box Controls
            SetupDecorationBoxes.FindDecorationControls(this);
        }

        /// <summary> 
        /// Loads the relevant menu contents when the visibility changes (user enters menu). 
        /// Saves and backs up when the user leaves for another menu (selects another tab). 
        /// </summary> 
        private void MenuVisibleChanged(object sender, EventArgs e)
        {
            // If set to visible 
            if (this.Visible)
            {
                // Set the titlebar.  
                Global.CurrentMenuName = "Reloaded Input Stack";
                Global.BaseForm.UpdateTitle("");

                // Initialize controller manager and populate controllers.
                controllerManager = new ControllerManager();
                PopulateControllers();
            }
            else
            {

            }
        }


        /// <summary>
        /// Populates all of the controller entries.
        /// </summary>
        private void PopulateControllers()
        {
            // Clear Controller List
            borderless_CurrentController.Items.Clear();

            // Populate Controller List with Names.
            foreach (IController controller in controllerManager.Controllers)
            {
                // Is controller XInput or DInput
                string controllerType = "";
                if (controller.Remapper.DeviceType == Remapper.InputDeviceType.XInput) { controllerType = "[XInput] "; }
                else if (controller.Remapper.DeviceType == Remapper.InputDeviceType.DirectInput) { controllerType = "[DInput] "; }

                // Add controller name from remapper.
                borderless_CurrentController.Items.Add(controllerType + controller.Remapper.GetControllerName);
            }
            
        }

    }
}
