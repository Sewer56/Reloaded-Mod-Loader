using ReloadedLauncher.Styles.Themes;
using ReloadedLauncher.Utilities.Windows;
using ReloadedLauncher.Windows.Children;
using Reloaded.Native;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static Reloaded.Misc.Config.ThemePropertyParser;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// This class provides a base form which should be copied for the generation of
    /// dialogs for the [Reloaded] Mod Loader UI.
    /// </summary>
    public partial class GetDestinationAxisDialog : Form, IDialog
    {
        #region Compositing
        /// <summary>
        /// Gets the creation parameters.
        /// The parameters are overridden to set the window as composited.
        /// Normally this would go into a child window class and other forms would
        /// derive from this, however this has shown to make the VS WinForm designer
        /// to be buggy.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | (int)WinAPI.WindowStyles.Constants.WS_EX_COMPOSITED;
                return cp;
            }
        }
        #endregion

        /// <summary>
        /// Initializes the form.
        /// </summary>
        public GetDestinationAxisDialog()
        {
            // Standard WinForms Init
            InitializeComponent();

            // Select textbox
            borderless_ValueBox.Select();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);
        }

        /// <summary>
        /// Initializes the form.
        /// </summary>
        /// <param name="initialValue">The initial value for the user to see.</param>
        public GetDestinationAxisDialog(string initialValue)
        {
            // Standard WinForms Init
            InitializeComponent();

            // Find initial value by matching strings.
            for (int x = 0; x < borderless_ValueBox.Items.Count; x++)
            {
                // Grab ComboBox value at index x
                string value = borderless_ValueBox.GetItemText(borderless_ValueBox.Items[x]);

                // If matches, select and exit.
                if (value == initialValue) { borderless_ValueBox.SelectedIndex = x; break; }
            }

            // Select ComboBox
            borderless_ValueBox.Select();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);
        }

        /// <summary>
        /// Spawns the dialog window prompts the user to enter the specified
        /// floating value to enter. Returns the results of the dialog back.
        /// </summary>
        /// <returns>Requested floating point number.</returns>
        public object GetValue()
        {
            // Show this dialog.
            this.ShowDialog();

            // Check if an object is selected, else retry recursively.
            if (borderless_ValueBox.SelectedIndex != -1)
            {
                return borderless_ValueBox.Text;
            }
            else
            {
                MessageBox.Show("You should select something, dummy.");
                return this.GetValue();
            }
        }

        /// <summary>
        /// Defines the title to be shown on the top of the dialog window.
        /// </summary>
        /// <param name="title">The title to be shown.</param>
        public void SetTitle(string title)
        {
            titleBar_Title.Text = title;
        }

        /// <summary>
        /// Load the global theme once the base form has finished loading (all MDI children should also have finished loading)
        /// by then, as they are loaded in the constructor, pretty convenient.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Set title bar colour.
            Global.BaseForm.panel_CategoryBar.BackColor = Theme.ThemeProperties.CategoryColours.BGColour;
            Global.BaseForm.panel_TitleBar.BackColor = Theme.ThemeProperties.TitleColours.BGColour;

            // Load the global theme.
            ApplyTheme.ThemeWindowsForm(this);
        }

        /// <summary>
        /// Called when the mouse is moved within the client area of the button while the
        /// left (or right depending on user setting) mouse is down.
        /// As the title is a button, which covers the entire top panel it
        /// effectively serves as the top panel in itself in receiving mouse events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleBarMouseDown(object sender, MouseEventArgs e) { MoveWindow.MoveTheWindow(this.Handle); }

        /// <summary>
        /// Close this window if the user hits ok.
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Close this window if the user presses the "Enter" key.
        /// </summary>
        private void ValueBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter)
            {
                this.Close();
            }
        }
    }
}
