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

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.Input;
using Reloaded.Input.Common;
using Reloaded.Input.Modules;
using Reloaded_GUI.Styles.Themes;
using ReloadedLauncher.Windows.Children.Dialogs.Input_Screen;
using Reloaded_GUI.Utilities.Controls;

namespace ReloadedLauncher.Windows.Children
{
    public partial class InputScreen : Form
    {
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
        /// Specifies the order of the rows
        /// in the button assignment DataGridView.
        /// </summary>
        public enum ButtonRows
        {
            ButtonA,
            ButtonB,
            ButtonX,
            ButtonY,
            ButtonLb,
            ButtonRb,
            ButtonLs,
            ButtonRs,
            ButtonBack,
            ButtonGuide,
            ButtonStart
        }

        /// <summary>
        /// Specifies the order of the rows
        /// in the emulation button assignment DataGridView.
        /// </summary>
        public enum EmulationRows
        {
            LeftStickUp,
            LeftStickDown,
            LeftStickLeft,
            LeftStickRight,
            RightStickUp,
            RightStickDown,
            RightStickLeft,
            RightStickRight,
            DPadUp,
            DPadDown,
            DPadLeft,
            DPadRight,
            LeftTrigger,
            RightTrigger
        }

        /// <summary>
        /// The amount of seconds the user is given for the
        /// remapping operation to set a new controller button.
        /// </summary>
        private const int RemapTimeout = 5;

        /// <summary>
        /// Task used for the asynchronous assignment of controller
        /// buttons or axis without the blocking of the UI thread.
        /// </summary>
        private static Task<bool> _controllerAssignTask;

        /// <summary>
        /// Set to false when a task starts executing and should be set to
        /// true in order to cancel any running task (following by waiting for exit).
        /// </summary>
        private static bool _taskCancellationToken;

        /// <summary>
        /// Stores the current timeout of the current button assignment operation.
        /// </summary>
        private static float _currentTimeout;

        /// <summary>
        /// Specifies a controller manager object,
        /// used for the management of game controllers.
        /// </summary>
        private ControllerManager _controllerManager;

        /// <summary>
        /// Thread used for the polling of the controller for a specific 
        /// controller port.
        /// </summary>
        private Thread _controllerPollThread;

        /// <summary>
        /// Sets the current controller port of the controller
        /// which is currently being previewed.
        /// </summary>
        private int _currentControllerPort = -1;

        /// <summary>
        /// Acts as the last controller index and is assigned
        /// at the same time that the index of the current controller
        /// combobox changes. Used for saving the last controller configuration.
        /// </summary>
        private int _lastControllerIndex = -1;

        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="mdiParent">The MDI Parent form, an instance of Base.cs</param>
        public InputScreen(Form mdiParent)
        {
            // Standard Winforms Initialization
            InitializeComponent();

            // Set the MDI parent
            MdiParent = mdiParent;

            // Add to the window list.
            Bindings.WindowsForms.Add(this);

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
            if (Visible)
            {
                // Set the titlebar.  
                Global.CurrentMenuName = "Reloaded Input Stack";
                Global.BaseForm.UpdateTitle("");
                
                // Setup controller preview.
                SetupControllerPreview();
            }
            else
            {
                // Save current controller and kill preview.
                SaveCurrentController();
                _controllerPollThread.Abort();
            }
        }

        /// <summary>
        /// Executed upon the loading of the input screen for the first time.
        /// </summary>
        private void Input_Screen_Load(object sender, EventArgs e)
        {
            // Initialize controller manager and populate controllers.
            _controllerManager = new ControllerManager();
            PopulateControllers();

            // Setup delegate for when controller is added/removed.
            _controllerManager.ControllerHotplugEventDelegate += PopulateControllers;
        }

        /// <summary>
        /// When the user presses the keyboard keys, 
        /// allow only numeric keypresses.
        /// </summary>
        private void ControllerPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the number is not a digit or control character (e.g. backspace) do nothing with it.
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        /// <summary>
        /// Ensure that the digit is a number when the user leaves the textbox.
        /// </summary>
        private void ControllerPort_Leave(object sender, EventArgs e)
        {
            // Check if string is numeric by parse attempt.
            var isNumeric = int.TryParse(borderless_ControllerPort.Text, out int n);

            // If it's not numeric, reset to 0.
            if (! isNumeric) borderless_ControllerPort.Text = "0";

            // Set the new controller port.
            _currentControllerPort = Convert.ToInt32(borderless_ControllerPort.Text);

            // Get Current Controller
            ControllerCommon.IController currentController = _controllerManager.Controllers[_lastControllerIndex];

            // Set Current Controller Port
            currentController.ControllerId = Convert.ToInt32(borderless_ControllerPort.Text);
        }

        /*
            LOAD/SAVE CONTROLLER INFORMATION
        */

        #region Load/Save Controller Information

        /// <summary>
        /// Populates all of the controller entries.
        /// </summary>
        private void PopulateControllers()
        {
            // Clear Controller List
            borderless_CurrentController.Items.Clear();

            // Populate Controller List with Names.
            foreach (ControllerCommon.IController controller in _controllerManager.Controllers)
            {
                // Is controller XInput or DInput
                string controllerType = "";
                if (controller.Remapper.DeviceType == Remapper.InputDeviceType.XInput)
                    controllerType = "[XInput] ";
                else if (controller.Remapper.DeviceType == Remapper.InputDeviceType.DirectInput) controllerType = "[DInput] ";

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
            ControllerCommon.IController controller = GetCurrentController();

            // Populate Button List.
            box_ButtonList.Rows.Add("BTN A:", controller.ButtonMapping.ButtonA);
            box_ButtonList.Rows.Add("BTN B:", controller.ButtonMapping.ButtonB);
            box_ButtonList.Rows.Add("BTN X:", controller.ButtonMapping.ButtonX);
            box_ButtonList.Rows.Add("BTN Y:", controller.ButtonMapping.ButtonY);
            box_ButtonList.Rows.Add("BTN LB:", controller.ButtonMapping.ButtonLb);
            box_ButtonList.Rows.Add("BTN RB:", controller.ButtonMapping.ButtonRb);
            box_ButtonList.Rows.Add("BTN LS:", controller.ButtonMapping.ButtonLs);
            box_ButtonList.Rows.Add("BTN RS:", controller.ButtonMapping.ButtonRs);
            box_ButtonList.Rows.Add("BTN BACK:", controller.ButtonMapping.ButtonBack);
            box_ButtonList.Rows.Add("BTN GUIDE:", controller.ButtonMapping.ButtonGuide);
            box_ButtonList.Rows.Add("BTN START:", controller.ButtonMapping.ButtonStart);
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
            ControllerCommon.IController controller = GetCurrentController();

            // Populate Button List.
            box_EmulatedAxisList.Rows.Add("LEFT STICK UP:", controller.EmulationMapping.LeftStickUp);
            box_EmulatedAxisList.Rows.Add("LEFT STICK DOWN:", controller.EmulationMapping.LeftStickDown);
            box_EmulatedAxisList.Rows.Add("LEFT STICK LEFT:", controller.EmulationMapping.LeftStickLeft);
            box_EmulatedAxisList.Rows.Add("LEFT STICK RIGHT:", controller.EmulationMapping.LeftStickRight);

            box_EmulatedAxisList.Rows.Add("RIGHT STICK UP:", controller.EmulationMapping.RightStickUp);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK DOWN:", controller.EmulationMapping.RightStickDown);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK LEFT:", controller.EmulationMapping.RightStickLeft);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK RIGHT:", controller.EmulationMapping.RightStickRight);

            box_EmulatedAxisList.Rows.Add("DPAD UP:", controller.EmulationMapping.DpadUp);
            box_EmulatedAxisList.Rows.Add("DPAD DOWN:", controller.EmulationMapping.DpadDown);
            box_EmulatedAxisList.Rows.Add("DPAD LEFT:", controller.EmulationMapping.DpadLeft);
            box_EmulatedAxisList.Rows.Add("DPAD RIGHT:", controller.EmulationMapping.DpadRight);

            box_EmulatedAxisList.Rows.Add("LEFT TRIGGER:", controller.EmulationMapping.LeftTrigger);
            box_EmulatedAxisList.Rows.Add("RIGHT TRIGGER:", controller.EmulationMapping.RightTrigger);
        }

        /// <summary>
        /// Populate the axis DataGridView content, with the appropriate
        /// axis > axis input mapping.
        /// </summary>
        private void PopulateAxis()
        {
            // Clear Button List
            box_AxisList.Rows.Clear();

            // Get Current Controller Entry
            ControllerCommon.IController controller = GetCurrentController();

            // Is XInput?
            bool isXInput = IsControllerXInput(controller);

            // Axis mapping entries.
            ControllerCommon.AxisMapping axisMappings = controller.AxisMapping;

            // Invividual mappings.
            ControllerCommon.AxisMappingEntry leftStickX = axisMappings.LeftStickX;
            ControllerCommon.AxisMappingEntry leftStickY = axisMappings.LeftStickY;
            ControllerCommon.AxisMappingEntry rightStickX = axisMappings.RightStickX;
            ControllerCommon.AxisMappingEntry rightStickY = axisMappings.RightStickY;
            ControllerCommon.AxisMappingEntry leftTrigger = axisMappings.LeftTrigger;
            ControllerCommon.AxisMappingEntry rightTrigger = axisMappings.RightTrigger;

            // Cell Order
            // Cells[0] Axis Name
            // Cells[1] Axis Property/Source Axis (DINPUT), N/A FOR XINPUT
            // Cells[2] Destination Axis (Axis the loader will treat current axis as)
            // Cells[3] Is Axis Inverted?
            // Cells[4] Deadzone % (0-100)
            // Cells[5] Radius Scale (Multiplier of analog values)

            // Get property names
            string leftStickXPropertyName = isXInput ? "N/A" : leftStickX.SourceAxis;
            string leftStickYPropertyName = isXInput ? "N/A" : leftStickY.SourceAxis;
            string rightStickXPropertyName = isXInput ? "N/A" : rightStickX.SourceAxis;
            string rightStickYPropertyName = isXInput ? "N/A" : rightStickY.SourceAxis;
            string leftTriggerPropertyName = isXInput ? "N/A" : leftTrigger.SourceAxis;
            string rightTriggerPropertyName = isXInput ? "N/A" : rightTrigger.SourceAxis;

            // Populate Button List.
            box_AxisList.Rows.Add("LEFT STICK X", leftStickXPropertyName, leftStickX.DestinationAxis, leftStickX.IsReversed, leftStickX.DeadZone, leftStickX.RadiusScale);
            box_AxisList.Rows.Add("LEFT STICK Y", leftStickYPropertyName, leftStickY.DestinationAxis, leftStickY.IsReversed, leftStickY.DeadZone, leftStickY.RadiusScale);

            box_AxisList.Rows.Add("RIGHT STICK X", rightStickXPropertyName, rightStickX.DestinationAxis, rightStickX.IsReversed, rightStickX.DeadZone, rightStickX.RadiusScale);
            box_AxisList.Rows.Add("RIGHT STICK Y", rightStickYPropertyName, rightStickY.DestinationAxis, rightStickY.IsReversed, rightStickY.DeadZone, rightStickY.RadiusScale);

            box_AxisList.Rows.Add("LEFT TRIGGER", leftTriggerPropertyName, leftTrigger.DestinationAxis, leftTrigger.IsReversed, leftTrigger.DeadZone, leftTrigger.RadiusScale);
            box_AxisList.Rows.Add("RIGHT TRIGGER", rightTriggerPropertyName, rightTrigger.DestinationAxis, rightTrigger.IsReversed, rightTrigger.DeadZone, rightTrigger.RadiusScale);
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

            // Set Current Controller Port
            borderless_ControllerPort.Text = Convert.ToString(GetCurrentController().ControllerId);

            // Set "last" controller index.
            _lastControllerIndex = borderless_CurrentController.SelectedIndex;

            // Set the new controller port.
            _currentControllerPort = Convert.ToInt32(borderless_ControllerPort.Text);
        }

        /// <summary>
        /// Saves the currently selected controller configuration.
        /// </summary>
        private void SaveCurrentController()
        {
            // If there is a previous controller selection
            if (_lastControllerIndex != -1)
            {
                // Get Controller
                ControllerCommon.IController currentController = _controllerManager.Controllers[_lastControllerIndex];

                // Set Only Controller Port, All Other Properties Were Already Saved when Set.
                currentController.ControllerId = Convert.ToInt32(borderless_ControllerPort.Text);

                // Save Controller Configuration to File
                currentController.Remapper.SetMappings();
            }
        }

        #endregion Load/Save Controller Information

        /*
            COMMON REMAPPING CODE
        */

        #region Common Remapping Code

        /// <summary>
        /// Retrieves the controller object for the currently selected controller.
        /// </summary>
        /// <returns>The controller object for the currently selected controller.</returns>
        private ControllerCommon.IController GetCurrentController()
        {   
            // Get current controller entry.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;
            return _controllerManager.Controllers[currentControllerEntry];
        }

        /// <summary>
        /// Returns true if the currently selected controller is an XInput Controller
        /// </summary>
        /// <param name="controller">The controller to check whether is or is not XInput</param>
        private bool IsControllerXInput(ControllerCommon.IController controller)
        {
            return controller.Remapper.DeviceType == Remapper.InputDeviceType.XInput;
        }

        /// <summary>
        /// Retrieves an axis mapping entry for the specified controller and row
        /// of the axis assignment DataGridView.
        /// </summary>
        /// <returns>An axis mapping entry for the current row.</returns>        
        /// <param name="axisRow">Axis mapping DataGridView row for which to obtain the axis mapping.</param>
        /// <param name="controller">The controller object to get axis mapping from.</param>
        private ControllerCommon.AxisMappingEntry GetAxisMappingEntry(ControllerCommon.IController controller, AxisRows axisRow)
        {
            // Obtain axis mapping to modify.
            switch (axisRow)
            {
                case AxisRows.LeftStickX:   return controller.AxisMapping.LeftStickX;
                case AxisRows.LeftStickY:   return controller.AxisMapping.LeftStickY;
                case AxisRows.RightStickX:  return controller.AxisMapping.RightStickX;
                case AxisRows.RightStickY:  return controller.AxisMapping.RightStickY;
                case AxisRows.LeftTrigger:  return controller.AxisMapping.LeftTrigger;
                case AxisRows.RightTrigger: return controller.AxisMapping.RightTrigger;
                default: return null;
            }
        }

        #endregion Common Remapping Code

        /*
            BUTTON REMAPPING CODE
        */

        #region Button Remapping Code

        /// <summary>
        /// Triggered upon having the user double click a cell of the button assignment box.
        /// </summary>
        private void ButtonList_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cancel current remapping task if running.
            CancelCurrentTask();

            // Get current controller
            ControllerCommon.IController controller = GetCurrentController(); 

            // Rebind <X> button.
            Task.Run( () => { ControllerButtonAssign(e.RowIndex, controller, sender); } );            
        }

        /// <summary>
        /// Polls the properties of the DirectInput controller and obtains a source button
        /// to assign to a specified user set button.
        /// </summary>
        /// <param name="row">The row of whose source button is to be changed.</param>
        /// <param name="controller">Specifies the current controller object.</param>
        /// <param name="dataGridView">The source datagridview with the source button field (either button or emulation)</param>
        private void ControllerButtonAssign(int row, ControllerCommon.IController controller, object dataGridView)
        {
            // Disable Mouse Tab Switching
            Global.BaseForm.EnableTabSwitching = false;

            // Get DatagridView Cell for Button
            DataGridView senderGridView = (DataGridView)dataGridView;
            DataGridViewCell sourceCell = senderGridView.Rows[row].Cells[1];

            // Thread to update current assignment cell.
            Thread updateCellThread = new Thread ( () =>
            {
                while (true)
                {
                    box_AxisList.Invoke((Action)delegate { sourceCell.Value = _currentTimeout.ToString("F2"); });
                    Thread.Sleep(16);
                }
            } );

            // Setup the button remap opertation.
            if (senderGridView == box_ButtonList)
                SetupButtonRemap((ButtonRows)row, controller);
            else if (senderGridView == box_EmulatedAxisList)
                SetupButtonRemap((EmulationRows)row, controller);
            else
                return;

            // Start tasks.
            updateCellThread.Start();
            _controllerAssignTask.Start();

            // Wait for the task to finish.
            _controllerAssignTask.Wait();
            updateCellThread.Abort();

            // Obtain task result (failed or succeeded remapping).
            bool successfulRemap = _controllerAssignTask.Result;

            // Refresh curent button on successful remap, else assign 255.
            if (successfulRemap) {
                if (senderGridView == box_ButtonList)
                    box_ButtonList.Invoke((Action)delegate { sourceCell.Value = GetButtonIndex((ButtonRows)row, controller); });
                else if (senderGridView == box_EmulatedAxisList) box_ButtonList.Invoke((Action)delegate { sourceCell.Value = GetButtonIndex((EmulationRows)row, controller); });
            }
            else { box_ButtonList.Invoke((Action)delegate { sourceCell.Value = "255"; }); }

            // Enable Mouse Tab Switching
            Global.BaseForm.EnableTabSwitching = true;
        }

        /// <summary>
        /// Retrieves the currently assigned button index to a specified row of the
        /// button rows DataGridView by grabbing the appropriate button mapping property
        /// from a passed in controller object.
        /// </summary>
        /// <param name="row">The row of the datagridview for which the appropriate button should be obtained.</param>
        /// <param name="controller">The controller to obtain button mapping from.</param>
        private int GetButtonIndex(ButtonRows row, ControllerCommon.IController controller)
        {
            switch (row)
            {
                case ButtonRows.ButtonA: return controller.ButtonMapping.ButtonA;
                case ButtonRows.ButtonB: return controller.ButtonMapping.ButtonB;
                case ButtonRows.ButtonX: return controller.ButtonMapping.ButtonX;
                case ButtonRows.ButtonY: return controller.ButtonMapping.ButtonY;
                case ButtonRows.ButtonLb: return controller.ButtonMapping.ButtonLb;
                case ButtonRows.ButtonRb: return controller.ButtonMapping.ButtonRb;
                case ButtonRows.ButtonLs: return controller.ButtonMapping.ButtonLs;
                case ButtonRows.ButtonRs: return controller.ButtonMapping.ButtonRs;
                case ButtonRows.ButtonBack: return controller.ButtonMapping.ButtonBack;
                case ButtonRows.ButtonGuide: return controller.ButtonMapping.ButtonGuide;
                case ButtonRows.ButtonStart: return controller.ButtonMapping.ButtonStart;
                default: return 255;
            }
        }


        /// <summary>
        /// Retrieves the currently assigned button index to a specified row of the
        /// emulation rows DataGridView by grabbing the appropriate button mapping property
        /// from a passed in controller object.
        /// </summary>
        /// <param name="row">The row of the datagridview for which the appropriate button should be obtained.</param>
        /// <param name="controller">The controller to obtain button mapping from.</param>
        private int GetButtonIndex(EmulationRows row, ControllerCommon.IController controller)
        {
            switch (row)
            {
                case EmulationRows.LeftStickUp: return controller.EmulationMapping.LeftStickUp;
                case EmulationRows.LeftStickDown: return controller.EmulationMapping.LeftStickDown;
                case EmulationRows.LeftStickLeft: return controller.EmulationMapping.LeftStickLeft;
                case EmulationRows.LeftStickRight: return controller.EmulationMapping.LeftStickRight;

                case EmulationRows.RightStickUp: return controller.EmulationMapping.RightStickUp;
                case EmulationRows.RightStickDown: return controller.EmulationMapping.RightStickDown;
                case EmulationRows.RightStickLeft: return controller.EmulationMapping.RightStickLeft;
                case EmulationRows.RightStickRight: return controller.EmulationMapping.RightStickRight;

                case EmulationRows.DPadUp: return controller.EmulationMapping.DpadUp;
                case EmulationRows.DPadDown: return controller.EmulationMapping.DpadDown;
                case EmulationRows.DPadLeft: return controller.EmulationMapping.DpadLeft;
                case EmulationRows.DPadRight: return controller.EmulationMapping.DpadRight;

                case EmulationRows.LeftTrigger: return controller.EmulationMapping.LeftTrigger;
                case EmulationRows.RightTrigger: return controller.EmulationMapping.RightTrigger;
                default: return 255;
            }
        }

        /// <summary>
        /// Sets up the Controller Assign Task to perform a task of remapping the
        /// currently assigned button for X to the specified row of the button
        /// DataGridView
        /// </summary>
        /// <param name="row">The row of the datagridview for which the appropriate button should be remapped.</param>
        /// <param name="controller">The controller to remap button mapping from.</param>
        private void SetupButtonRemap(ButtonRows row, ControllerCommon.IController controller)
        {
            // Remap specified button
            switch (row)
            {
                case ButtonRows.ButtonA: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonA, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonB: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonB, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonX: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonX, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonY: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonY, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonLb: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonLb, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonRb: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonRb, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonLs: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonLs, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonRs: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonRs, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonBack: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonBack, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonGuide: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonGuide, ref _taskCancellationToken)); break;
                case ButtonRows.ButtonStart: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.ButtonMapping.ButtonStart, ref _taskCancellationToken)); break;
            }
        }

        /// <summary>
        /// Sets up the Controller Assign Task to perform a task of remapping the
        /// currently assigned button for X to the specified row of the emulation
        /// DataGridView
        /// </summary>
        /// <param name="row">The row of the datagridview for which the appropriate button should be remapped.</param>
        /// <param name="controller">The controller to remap button mapping from.</param>
        private void SetupButtonRemap(EmulationRows row, ControllerCommon.IController controller)
        {
            // Remap specified button
            switch (row)
            {
                case EmulationRows.LeftStickUp: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.LeftStickUp, ref _taskCancellationToken)); break;
                case EmulationRows.LeftStickDown: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.LeftStickDown, ref _taskCancellationToken)); break;
                case EmulationRows.LeftStickLeft: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.LeftStickLeft, ref _taskCancellationToken)); break;
                case EmulationRows.LeftStickRight: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.LeftStickRight, ref _taskCancellationToken)); break;

                case EmulationRows.RightStickUp: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.RightStickUp, ref _taskCancellationToken)); break;
                case EmulationRows.RightStickDown: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.RightStickDown, ref _taskCancellationToken)); break;
                case EmulationRows.RightStickLeft: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.RightStickLeft, ref _taskCancellationToken)); break;
                case EmulationRows.RightStickRight: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.RightStickRight, ref _taskCancellationToken)); break;

                case EmulationRows.DPadUp: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.DpadUp, ref _taskCancellationToken)); break;
                case EmulationRows.DPadDown: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.DpadDown, ref _taskCancellationToken)); break;
                case EmulationRows.DPadLeft: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.DpadLeft, ref _taskCancellationToken)); break;
                case EmulationRows.DPadRight: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.DpadRight, ref _taskCancellationToken)); break;

                case EmulationRows.LeftTrigger: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.LeftTrigger, ref _taskCancellationToken)); break;
                case EmulationRows.RightTrigger: _controllerAssignTask = new Task<bool>(() => controller.RemapButtons(RemapTimeout, out _currentTimeout, ref controller.EmulationMapping.RightTrigger, ref _taskCancellationToken)); break;
            }
        }

        /// <summary>
        /// Allows for the resetting of a button to 255 if the middle mouse button is pressed.
        /// </summary>
        private void ButtonList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                // Cast the DataGridView
                DataGridView dataGridView = (DataGridView)sender;

                // Set button index to 255 on GUI
                int currentRow = dataGridView.SelectedCells[0].RowIndex;
                dataGridView.Rows[currentRow].Cells[1].Value = "255";

                // Disable binding from non GUI elements
                if (dataGridView == box_ButtonList)
                    DisableButtonRow((ButtonRows)currentRow);
                else if (dataGridView == box_EmulatedAxisList) DisableEmulatedRow((EmulationRows)currentRow);
            }

        }

        /// <summary>
        /// Sets the current button mapping to 255 (disabled) for a specific row passed in row.
        /// </summary>
        /// <param name="buttonRow">Specifies the row that is to have the current button binding disabled.</param>
        private void DisableButtonRow(ButtonRows buttonRow)
        {
            switch (buttonRow)
            {
                case ButtonRows.ButtonA: GetCurrentController().ButtonMapping.ButtonA = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonB: GetCurrentController().ButtonMapping.ButtonB = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonX: GetCurrentController().ButtonMapping.ButtonX = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonY: GetCurrentController().ButtonMapping.ButtonY = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonLb: GetCurrentController().ButtonMapping.ButtonLb = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonRb: GetCurrentController().ButtonMapping.ButtonRb = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonLs: GetCurrentController().ButtonMapping.ButtonLs = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonRs: GetCurrentController().ButtonMapping.ButtonRs = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonBack: GetCurrentController().ButtonMapping.ButtonBack = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonGuide: GetCurrentController().ButtonMapping.ButtonGuide = ControllerCommon.ButtonNull; break;
                case ButtonRows.ButtonStart: GetCurrentController().ButtonMapping.ButtonStart = ControllerCommon.ButtonNull; break;
            }
        }

        /// <summary>
        /// Sets the current emulation mapping to 255 (disabled) for a specific row passed in row.
        /// </summary>
        /// <param name="emulationRow">Specifies the row that is to have the current emulation binding disabled.</param>
        private void DisableEmulatedRow(EmulationRows emulationRow)
        {
            switch (emulationRow)
            {
                case EmulationRows.DPadUp: GetCurrentController().EmulationMapping.DpadUp = ControllerCommon.ButtonNull; break;
                case EmulationRows.DPadDown: GetCurrentController().EmulationMapping.DpadDown = ControllerCommon.ButtonNull; break;
                case EmulationRows.DPadLeft: GetCurrentController().EmulationMapping.DpadLeft = ControllerCommon.ButtonNull; break;
                case EmulationRows.DPadRight: GetCurrentController().EmulationMapping.DpadRight = ControllerCommon.ButtonNull; break;

                case EmulationRows.LeftStickUp: GetCurrentController().EmulationMapping.LeftStickUp = ControllerCommon.ButtonNull; break;
                case EmulationRows.LeftStickDown: GetCurrentController().EmulationMapping.LeftStickDown = ControllerCommon.ButtonNull; break;
                case EmulationRows.LeftStickLeft: GetCurrentController().EmulationMapping.LeftStickLeft = ControllerCommon.ButtonNull; break;
                case EmulationRows.LeftStickRight: GetCurrentController().EmulationMapping.LeftStickRight = ControllerCommon.ButtonNull; break;

                case EmulationRows.RightStickUp: GetCurrentController().EmulationMapping.RightStickUp = ControllerCommon.ButtonNull; break;
                case EmulationRows.RightStickDown: GetCurrentController().EmulationMapping.RightStickDown = ControllerCommon.ButtonNull; break;
                case EmulationRows.RightStickLeft: GetCurrentController().EmulationMapping.RightStickLeft = ControllerCommon.ButtonNull; break;
                case EmulationRows.RightStickRight: GetCurrentController().EmulationMapping.RightStickRight = ControllerCommon.ButtonNull; break;

                case EmulationRows.LeftTrigger: GetCurrentController().EmulationMapping.LeftTrigger = ControllerCommon.ButtonNull; break;
                case EmulationRows.RightTrigger: GetCurrentController().EmulationMapping.RightTrigger = ControllerCommon.ButtonNull; break;
            }
        }

        #endregion Button Remapping Code

        /*
            AXIS REMAPPING CODE
        */

        #region Axis Remapping Code

        /// <summary>
        /// Triggered upon having the user double click a cell of the axis assignment box.
        /// </summary>
        /// <param name="sender">The DataGridView that called the message.</param>
        /// <param name="e"></param>
        private void AxisBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the row is the axis header, if it is, ignore
            if (e.RowIndex == -1) { return; }

            // Pass event to relevant column by index.
            switch (e.ColumnIndex)
            {
                // Remap Source Axis With Controller
                case (int)AxisColumns.AxisSource:

                    // Cancel current remapping.
                    CancelCurrentTask();

                    // Obtain controller object for remapping.
                    ControllerCommon.IController controller = GetCurrentController();

                    // Remap the controller
                    Task.Run(() => { ControllerSourceAxisAssign((AxisRows)e.RowIndex, controller); });
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
            if (_controllerAssignTask != null)
            {
                _taskCancellationToken = true;
                _controllerAssignTask.Wait();
                _taskCancellationToken = false;
            }
        }

        /// <summary>
        /// Polls the properties of the DirectInput controller and obtains a source axis.
        /// </summary>
        /// <param name="row">The row of whose source axis is to be changed.</param>
        /// <param name="controller">Specifies the current controller object.</param>
        private void ControllerSourceAxisAssign(AxisRows row, ControllerCommon.IController controller)
        {
            // Do not execute if the controller is XInput (XInput axis are pre-programmed).
            if (IsControllerXInput(controller)) return;

            // Get row and cell to check (less verbose)
            int axisRow = (int)row;
            int axisColumn = (int)AxisColumns.AxisSource;

            // Get DatagridView Cell for Source Axis
            DataGridViewCell sourceCell = box_AxisList.Rows[axisRow].Cells[axisColumn];

            // Thread to update current assignment cell.
            Thread updateCellThread = new Thread ( () => 
            {
                while (true)
                {
                    box_AxisList.Invoke((Action)delegate { sourceCell.Value = _currentTimeout.ToString("F2"); });
                    Thread.Sleep(16);
                }
            } );

            // Start the thread
            // Remark: The thread will be stopped after the user either presses a key or after timeout.
            updateCellThread.Start();

            // Remapping Operation Commence.
            ControllerCommon.AxisMappingEntry axisMapping = GetAxisMappingEntry(controller, row);

            // Set Task
            _controllerAssignTask = new Task<bool>(() => controller.RemapAxis(RemapTimeout, out _currentTimeout, axisMapping, ref _taskCancellationToken));

            // Start remapping polling task.
            _controllerAssignTask.Start();

            // Wait for the task to finish.
            _controllerAssignTask.Wait();
            updateCellThread.Abort();

            // Obtain task result (failed or succeeded remapping).
            bool successfulRemap = _controllerAssignTask.Result;

            // Update if successfully remapped.
            sourceCell.Value = successfulRemap ? axisMapping.SourceAxis : "Null";
        }

        #endregion Axis Remapping Code

        /*
            AXIS PROPERTIES
        */

        #region Axis Properties

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
            ControllerCommon.ControllerAxis returnedAxis = (ControllerCommon.ControllerAxis)Enum.Parse(typeof(ControllerCommon.ControllerAxis), (string)deadzoneNumberDialog.GetValue());
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDestination].Value = returnedAxis;

            // Set value to axis mapping entry.
            ControllerCommon.IController currentController = GetCurrentController();
            ControllerCommon.AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.DestinationAxis = returnedAxis;
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
            isEnabled = ! isEnabled;

            // Write back the row value.
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisInverted].Value = isEnabled;

            // Set value to axis mapping entry.
            ControllerCommon.IController currentController = GetCurrentController();
            ControllerCommon.AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.IsReversed = isEnabled;
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
            float deadZone = (float)deadzoneNumberDialog.GetValue();
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDeadzone].Value = deadZone;

            // Set value to axis mapping entry.
            ControllerCommon.IController currentController = GetCurrentController();
            ControllerCommon.AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.DeadZone = deadZone;
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
            float radius = (float)deadzoneNumberDialog.GetValue();
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisRadiusScale].Value = radius;

            // Set value to axis mapping entry.
            ControllerCommon.IController currentController = GetCurrentController();
            ControllerCommon.AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.RadiusScale = radius;
        }

        #endregion Axis Properties

        /*
            CONTROLLER INPUT PREVIEW INFORMATION
        */

        #region Controller Input Preview

        /// <summary>
        /// Sets up the controller preview thread which will show controller inputs
        /// in real time for controllers at set specified ports.
        /// </summary>
        private void SetupControllerPreview()
        {
            _controllerPollThread = new Thread
            (
                () =>
                {
                    while(true)
                    {
                        ShowControllerPreview();
                        Thread.Sleep(16);
                    }
                }
            );
            _controllerPollThread.Start();
        }

        /// <summary>
        /// Displays a preview of the currently applied controls to a specified controller port.
        /// </summary>
        private void ShowControllerPreview()
        {
            // Get the current set controller inputs for preview.
            ControllerCommon.ControllerInputs controllerInputs = _controllerManager.GetInput(_currentControllerPort);

            // Set the analog indicator values.
            SetAnalogIndicatorValues(controllerInputs);

            // Set trigger indicator values.
            SetTriggers(controllerInputs);

            // Set buttons.
            SetButtons(controllerInputs);
        }

        /// <summary>
        /// Sets the status of the analog stick indicators on the preview section
        /// of the input window. Moves the indicator appropriately to respond
        /// to the realtime axis changes.
        /// </summary>
        /// <param name="controllerInputs">The structure containing the current controller inputs.</param>
        private void SetAnalogIndicatorValues(ControllerCommon.ControllerInputs controllerInputs)
        {
            // Retrieve analog stick X and Y values.
            float analogStickX = controllerInputs.LeftStick.GetX();
            float analogStickY = controllerInputs.LeftStick.GetY();
            float analogStickRx = controllerInputs.RightStick.GetX();
            float analogStickRy = controllerInputs.RightStick.GetY();

            // Scale analog values to controls.
            analogStickX = analogStickX / ControllerCommon.AxisMaxValueF * borderless_LeftStick.MAX_VALUE_RADIUS;
            analogStickY = analogStickY / ControllerCommon.AxisMaxValueF * borderless_LeftStick.MAX_VALUE_RADIUS;
            analogStickRx = analogStickRx / ControllerCommon.AxisMaxValueF * borderless_LeftStick.MAX_VALUE_RADIUS;
            analogStickRy = analogStickRy / ControllerCommon.AxisMaxValueF * borderless_LeftStick.MAX_VALUE_RADIUS;

            // Set analog sticks
            borderless_LeftStick.SetXYValue((int)analogStickX, (int)analogStickY);
            borderless_RightStick.SetXYValue((int)analogStickRx, (int)analogStickRy);
        }

        /// <summary>
        /// Sets the status of the trigger pressure indicators on the preview section
        /// of the input window. Moves the bar height appropriately in response to the realtime
        /// axis changes.
        /// </summary>
        /// <param name="controllerInputs">The structure containing the current controller inputs.</param>
        private void SetTriggers(ControllerCommon.ControllerInputs controllerInputs)
        {
            // Retrieve trigger pressure values.
            float leftTriggerPressure = controllerInputs.GetLeftTriggerPressure();
            float rightTriggerPressure = controllerInputs.GetRightTriggerPressure();

            // Scale trigger pressure values to controls.
            leftTriggerPressure = leftTriggerPressure / ControllerCommon.AxisMaxValueF * borderless_LeftStick.MAX_VALUE_RADIUS;
            rightTriggerPressure = rightTriggerPressure / ControllerCommon.AxisMaxValueF * borderless_LeftStick.MAX_VALUE_RADIUS;

            // Set trigger indicator
            borderless_LeftTrigger.Value = (int)leftTriggerPressure;
            borderless_RightTrigger.Value = (int)rightTriggerPressure;
        }

        /// <summary>
        /// Sets the status of the button press indicators on the preview section
        /// of the input window. Swaps the background and foreground colour appropriately
        /// to respond to whether a specific button is pressed or not.
        /// </summary>
        /// <param name="controllerInputs">The structure containing the current controller inputs.</param>
        private void SetButtons(ControllerCommon.ControllerInputs controllerInputs)
        {
            // Set button press states for each button.
            borderless_ButtonA.ButtonEnabled = controllerInputs.ControllerButtons.ButtonA;
            borderless_ButtonB.ButtonEnabled = controllerInputs.ControllerButtons.ButtonB;
            borderless_ButtonX.ButtonEnabled = controllerInputs.ControllerButtons.ButtonX;
            borderless_ButtonY.ButtonEnabled = controllerInputs.ControllerButtons.ButtonY;
            borderless_ButtonL.ButtonEnabled = controllerInputs.ControllerButtons.ButtonLb;
            borderless_ButtonR.ButtonEnabled = controllerInputs.ControllerButtons.ButtonRb;

            borderless_ButtonUP.ButtonEnabled = controllerInputs.ControllerButtons.DpadUp;
            borderless_ButtonDOWN.ButtonEnabled = controllerInputs.ControllerButtons.DpadDown;
            borderless_ButtonLEFT.ButtonEnabled = controllerInputs.ControllerButtons.DpadLeft;
            borderless_ButtonRIGHT.ButtonEnabled = controllerInputs.ControllerButtons.DpadRight;
            borderless_ButtonSELECT.ButtonEnabled = controllerInputs.ControllerButtons.ButtonBack;
            borderless_ButtonSTART.ButtonEnabled = controllerInputs.ControllerButtons.ButtonStart;
        }

        #endregion Controller Input Preview
    }
}
