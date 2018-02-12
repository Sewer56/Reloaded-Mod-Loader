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
using ReloadedLauncher.Windows.Children.Dialogs;
using System.Threading.Tasks;

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
        /// Task used for the asynchronous assignment of controller
        /// buttons or axis without the blocking of the UI thread.
        /// </summary>
        static Task controllerAssignTask;

        /// <summary>
        /// Set to false when a task starts executing and should be set to
        /// true in order to cancel any running task (following by waiting for exit).
        /// </summary>
        static bool taskCancellationToken;

        /// <summary>
        /// Enumerator specifying the order of the axis
        /// columns, left to right.
        /// </summary>
        public enum AxisColumns
        {
            AxisName,
            AxisSource,
            AxisDestination,
            AxisInverted,
            AxisDeadzone,
            AxisRadiusScale
        }
        
        /// <summary>
        /// Specifies the row order of the axis DataGridView
        /// rows, up to down.
        /// </summary>
        public enum AxisRows
        {
            LeftStickX,
            LeftStickY,
            RightStickX,
            RightStickY,
            LeftTrigger,
            RightTrigger
        }

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

                // Populate buttons.
                PopulateButtons();

                // Populate emulated axis.
                PopulateEmulatedAxis();

                // Populate axis.
                PopulateAxis();
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

            // Select first item.
            borderless_CurrentController.SelectedIndex = 0;
        }

        /// <summary>
        /// Populate the button DataGridView content, with both the buttons to assign
        /// and the current already assigned buttons.
        /// </summary>
        private void PopulateButtons()
        {
            // Clear Button List
            box_ButtonList.Rows.Clear();

            // Current controller entry.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;

            // Populate Button List.
            box_ButtonList.Rows.Add("BTN A:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_A);
            box_ButtonList.Rows.Add("BTN B:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_B);
            box_ButtonList.Rows.Add("BTN X:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_X);
            box_ButtonList.Rows.Add("BTN Y:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_Y);
            box_ButtonList.Rows.Add("BTN LB:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_LB);
            box_ButtonList.Rows.Add("BTN RB:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_RB);
            box_ButtonList.Rows.Add("BTN LS:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_LS);
            box_ButtonList.Rows.Add("BTN RS:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_RS);
            box_ButtonList.Rows.Add("BTN BACK:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_Back);
            box_ButtonList.Rows.Add("BTN GUIDE:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_Guide);
            box_ButtonList.Rows.Add("BTN START:", controllerManager.Controllers[currentControllerEntry].ButtonMapping.Button_Start);
        }

        /// <summary>
        /// Populate the emulated axis DataGridView content, with the appropriate
        /// button > axis input support (and DPAD).
        /// </summary>
        private void PopulateEmulatedAxis()
        {
            // Clear Button List
            box_EmulatedAxisList.Rows.Clear();

            // Current controller entry.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;

            // Populate Button List.
            box_EmulatedAxisList.Rows.Add("LEFT STICK UP:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Left_Stick_Up);
            box_EmulatedAxisList.Rows.Add("LEFT STICK DOWN:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Left_Stick_Down);
            box_EmulatedAxisList.Rows.Add("LEFT STICK LEFT:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Left_Stick_Left);
            box_EmulatedAxisList.Rows.Add("LEFT STICK RIGHT:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Left_Stick_Right);

            box_EmulatedAxisList.Rows.Add("RIGHT STICK UP:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Right_Stick_Up);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK DOWN:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Right_Stick_Down);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK LEFT:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Right_Stick_Left);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK RIGHT:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Right_Stick_Right);

            box_EmulatedAxisList.Rows.Add("DPAD UP:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.DPAD_UP);
            box_EmulatedAxisList.Rows.Add("DPAD DOWN:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.DPAD_DOWN);
            box_EmulatedAxisList.Rows.Add("DPAD LEFT:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.DPAD_LEFT);
            box_EmulatedAxisList.Rows.Add("DPAD RIGHT:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.DPAD_RIGHT);

            box_EmulatedAxisList.Rows.Add("LEFT TRIGGER:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Left_Trigger);
            box_EmulatedAxisList.Rows.Add("RIGHT TRIGGER:", controllerManager.Controllers[currentControllerEntry].EmulationMapping.Right_Trigger);
        }

        /// <summary>
        /// Populate the axis DataGridView content, with the appropriate
        /// axis > axis input mapping.
        /// </summary>
        private void PopulateAxis()
        {
            // Clear Button List
            box_AxisList.Rows.Clear();

            // Current controller entry.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;

            // Is XInput?
            bool isXInput = IsControllerXInput();

            // Axis mapping entries.
            AxisMapping axisMappings = controllerManager.Controllers[currentControllerEntry].AxisMapping;

            // Invividual mappings.
            AxisMappingEntry leftStickX = axisMappings.leftStickX;
            AxisMappingEntry leftStickY = axisMappings.leftStickY;
            AxisMappingEntry rightStickX = axisMappings.rightStickX;
            AxisMappingEntry rightStickY = axisMappings.rightStickY;
            AxisMappingEntry leftTrigger = axisMappings.leftTrigger;
            AxisMappingEntry rightTrigger = axisMappings.rightTrigger;

            // Cell Order
            // Cells[0] Axis Name
            // Cells[1] Axis Property/Source Axis (DINPUT), N/A FOR XINPUT
            // Cells[2] Destination Axis (Axis the loader will treat current axis as)
            // Cells[3] Is Axis Inverted?
            // Cells[4] Deadzone % (0-100)
            // Cells[5] Radius Scale (Multiplier of analog values)

            // Get property names
            string leftStickXPropertyName = isXInput ? "N/A" : leftStickX.propertyName;
            string leftStickYPropertyName = isXInput ? "N/A" : leftStickY.propertyName;
            string rightStickXPropertyName = isXInput ? "N/A" : rightStickX.propertyName;
            string rightStickYPropertyName = isXInput ? "N/A" : rightStickY.propertyName;
            string leftTriggerPropertyName = isXInput ? "N/A" : leftTrigger.propertyName;
            string rightTriggerPropertyName = isXInput ? "N/A" : rightTrigger.propertyName;

            // Populate Button List.
            box_AxisList.Rows.Add("LEFT STICK X", leftStickXPropertyName, leftStickX.axis, leftStickX.isReversed, leftStickX.deadZone, leftStickX.radiusScale);
            box_AxisList.Rows.Add("LEFT STICK Y", leftStickYPropertyName, leftStickY.axis, leftStickY.isReversed, leftStickY.deadZone, leftStickY.radiusScale);

            box_AxisList.Rows.Add("RIGHT STICK X", rightStickXPropertyName, rightStickX.axis, rightStickX.isReversed, rightStickX.deadZone, rightStickX.radiusScale);
            box_AxisList.Rows.Add("RIGHT STICK Y", rightStickYPropertyName, rightStickY.axis, rightStickY.isReversed, rightStickY.deadZone, rightStickY.radiusScale);

            box_AxisList.Rows.Add("LEFT TRIGGER", leftTriggerPropertyName, leftTrigger.axis, leftTrigger.isReversed, leftTrigger.deadZone, leftTrigger.radiusScale);
            box_AxisList.Rows.Add("RIGHT TRIGGER", rightTriggerPropertyName, rightTrigger.axis, rightTrigger.isReversed, rightTrigger.deadZone, rightTrigger.radiusScale);
        }

        /// <summary>
        /// Triggered upon having the user double click a cell of the axis assignment box.
        /// </summary>
        /// <param name="sender">The DataGridView that called the message.</param>
        /// <param name="e"></param>
        private void AxisBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pass event to relevant column by index.
            switch (e.ColumnIndex)
            {
                case (int)AxisColumns.AxisSource:
                    // Cancel current task (if available)
                    CancelCurrentTask();
                    controllerAssignTask = Task.Run(() => { ControllerSourceAxisAssign((AxisRows)e.RowIndex); } );
                    break;
                case (int)AxisColumns.AxisDestination: ControllerDestinationAxisAssign((AxisRows)e.RowIndex); break;
                case (int)AxisColumns.AxisInverted: ControllerInvertAxis((AxisRows)e.RowIndex); break;
                case (int)AxisColumns.AxisDeadzone: ControllerSetAxisDeadzone((AxisRows)e.RowIndex); break;
                case (int)AxisColumns.AxisRadiusScale: ControllerSetAxisRadius((AxisRows)e.RowIndex); break;
            }

        }

        /// <summary>
        /// Cancels the current ongoing task and waits for it to finish execution.
        /// </summary>
        private void CancelCurrentTask()
        {
            if (controllerAssignTask != null)
            {
                taskCancellationToken = true;
                controllerAssignTask.Wait();
            }
        }

        /// <summary>
        /// Polls the properties of the DirectInput controller and obtains a source axis.
        /// </summary>
        /// <param name="row">The row of whose source axis is to be changed.</param>
        private async void ControllerSourceAxisAssign(AxisRows row)
        {
            // Get row and cell to check (less verbose)
            int axisRow = (int)row;
            int axisColumn = (int)AxisColumns.AxisSource;

            // Get DatagridView Cell for Source Axis
            DataGridViewCell sourceCell = box_AxisList.Rows[axisRow].Cells[axisColumn];

            // Get Current Controller.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;
            IController controller = controllerManager.Controllers[currentControllerEntry];

            // Start up Remapping Operation.

            // Check for task cancellation.
            if (taskCancellationToken) { return; }
        }

        /// <summary>
        /// Displays a dialog allowing the user to change the destination axis of the controller.
        /// </summary>
        /// <param name="axisRow">The axis of the destination specifying which axis to recognize the specified source axis as.</param>
        private void ControllerDestinationAxisAssign(AxisRows axisRow)
        {
            // Get initial value
            string initialValue = Convert.ToString(box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDestination].Value);

            // Open the dialog to get new number.
            GetDestinationAxisDialog deadzoneNumberDialog = new GetDestinationAxisDialog(initialValue);
            deadzoneNumberDialog.StartPosition = FormStartPosition.CenterParent;
            deadzoneNumberDialog.SetTitle("Set Destination Axis!");

            // Set new value
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDestination].Value = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), (string)deadzoneNumberDialog.GetValue());
        }

        /// <summary>
        /// Inverts the directionality of the passed in axis at the specified row.
        /// </summary>
        /// <param name="axisRow">The row whose directionality is to be changed.</param>
        private void ControllerInvertAxis(AxisRows axisRow)
        {
            // Obtain current string
            bool isEnabled = (bool)box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisInverted].Value;

            // Invert True/False value
            if (! isEnabled) { isEnabled = true; } else { isEnabled = false; }

            // Write back the row value.
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisInverted].Value = isEnabled;
        }

        /// <summary>
        /// Displays a dialog allowing the user to change the deadzone of the controller.
        /// </summary>
        /// <param name="axisRow">The row whose deadzone is to be changed.</param>
        private void ControllerSetAxisDeadzone(AxisRows axisRow)
        {
            // Get initial value
            float getInitialValue = (float)box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDeadzone].Value;

            // Open the dialog to get new number.
            GetNumberDialog deadzoneNumberDialog = new GetNumberDialog(getInitialValue);
            deadzoneNumberDialog.StartPosition = FormStartPosition.CenterParent;
            deadzoneNumberDialog.SetTitle("Modify Controller Deadzone!");

            // Set new value
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDeadzone].Value = deadzoneNumberDialog.GetValue();
        }

        /// <summary>
        /// Displays a dialog allowing the user to change the radius of the controller.
        /// </summary>
        /// <param name="axisRow">The row whose radius is to be changed.</param>
        private void ControllerSetAxisRadius(AxisRows axisRow)
        {
            // Get initial value
            float initialValue = (float)box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisRadiusScale].Value;

            // Open the dialog to get new number.
            GetNumberDialog deadzoneNumberDialog = new GetNumberDialog(initialValue);
            deadzoneNumberDialog.StartPosition = FormStartPosition.CenterParent;
            deadzoneNumberDialog.SetTitle("Modify Controller Radius!");

            // Set new value
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisRadiusScale].Value = deadzoneNumberDialog.GetValue();
        }

        /// <summary>
        /// Switches the currently shown controller when the user
        /// changes the value of the ComboBox dropdown.
        /// </summary>
        private void CurrentController_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save Current Controller
            SaveCurrentController();

            // Populate buttons.
            PopulateButtons();

            // Populate emulated axis.
            PopulateEmulatedAxis();

            // Populate axis.
            PopulateAxis();
        }

        /// <summary>
        /// Returns true if the currently selected controller is an XInput Controller
        /// </summary>
        private bool IsControllerXInput()
        {
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;
            return controllerManager.Controllers[currentControllerEntry].Remapper.DeviceType == Remapper.InputDeviceType.XInput ? true : false;
        }

        /// <summary>
        /// Saves the currently selected controller configuration.
        /// </summary>
        private void SaveCurrentController()
        {

        }
    }
}
