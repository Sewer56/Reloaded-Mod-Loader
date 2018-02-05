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

namespace ReloadedLauncher.Windows.Children
{
    public partial class About_Screen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="MDIParent">The MDI Parent form, an instance of Base.cs</param>
        public About_Screen(Form MDIParent)
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
                Global.CurrentMenuName = "About [Reloaded]";
                Global.BaseForm.UpdateTitle("");

                // Set version
                item_Version.Text = "Version: " + Application.ProductVersion.ToString() + " | Compiled on " + File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString(); 
            }
        }
    }
}
