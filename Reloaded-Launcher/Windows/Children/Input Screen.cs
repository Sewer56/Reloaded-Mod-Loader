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

using ReloadedLauncher.Utilities.Controls;
using ReloadedLauncher.Windows.Children.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        /// Thread used for the polling of the controller for a specific 
        /// controller port.
        /// </summary>
        Thread controllerPollThread;

        /// <summary>
        /// The amount of seconds the user is given for the
        /// remapping operation to set a new controller button.
        /// </summary>
        const int REMAP_TIMEOUT = 5;

        /// <summary>
        /// Acts as the last controller index and is assigned
        /// at the same time that the index of the current controller
        /// combobox changes. Used for saving the last controller configuration.
        /// </summary>
        int lastControllerIndex = -1;

        /// <summary>
        /// Sets the current controller port of the controller
        /// which is currently being previewed.
        /// </summary>
        int currentControllerPort = -1;

        /// <summary>
        /// Task used for the asynchronous assignment of controller
        /// buttons or axis without the blocking of the UI thread.
        /// </summary>
        static Task<bool> controllerAssignTask;

        /// <summary>
        /// Set to false when a task starts executing and should be set to
        /// true in order to cancel any running task (following by waiting for exit).
        /// </summary>
        static bool taskCancellationToken;

        /// <summary>
        /// Stores the current timeout of the current button assignment operation.
        /// </summary>
        static float CurrentTimeout;

        /// <summary>
        /// Standard parameterless delegate which returns
        /// type bool(ean).
        /// </summary>
        delegate bool BooleanDelegate();

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
            ButtonLB,
            ButtonRB,
            ButtonLS,
            ButtonRS,
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


            }
            else
            {
                SaveCurrentController();
            }
        }

        /// <summary>
        /// Executed upon the loading of the input screen for the first time.
        /// </summary>
        private void Input_Screen_Load(object sender, EventArgs e)
        {
            // Initialize controller manager and populate controllers.
            controllerManager = new ControllerManager();
            PopulateControllers();

            // Setup delegate for when controller is added/removed.
            controllerManager.ControllerHotplugEventDelegate += PopulateControllers;

            // Setup Controller Preview   
            SetupControllerPreview();
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
            IController controller = GetCurrentController();

            // Populate Button List.
            box_ButtonList.Rows.Add("BTN A:", controller.ButtonMapping.Button_A);
            box_ButtonList.Rows.Add("BTN B:", controller.ButtonMapping.Button_B);
            box_ButtonList.Rows.Add("BTN X:", controller.ButtonMapping.Button_X);
            box_ButtonList.Rows.Add("BTN Y:", controller.ButtonMapping.Button_Y);
            box_ButtonList.Rows.Add("BTN LB:", controller.ButtonMapping.Button_LB);
            box_ButtonList.Rows.Add("BTN RB:", controller.ButtonMapping.Button_RB);
            box_ButtonList.Rows.Add("BTN LS:", controller.ButtonMapping.Button_LS);
            box_ButtonList.Rows.Add("BTN RS:", controller.ButtonMapping.Button_RS);
            box_ButtonList.Rows.Add("BTN BACK:", controller.ButtonMapping.Button_Back);
            box_ButtonList.Rows.Add("BTN GUIDE:", controller.ButtonMapping.Button_Guide);
            box_ButtonList.Rows.Add("BTN START:", controller.ButtonMapping.Button_Start);
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
            IController controller = GetCurrentController();

            // Populate Button List.
            box_EmulatedAxisList.Rows.Add("LEFT STICK UP:", controller.EmulationMapping.Left_Stick_Up);
            box_EmulatedAxisList.Rows.Add("LEFT STICK DOWN:", controller.EmulationMapping.Left_Stick_Down);
            box_EmulatedAxisList.Rows.Add("LEFT STICK LEFT:", controller.EmulationMapping.Left_Stick_Left);
            box_EmulatedAxisList.Rows.Add("LEFT STICK RIGHT:", controller.EmulationMapping.Left_Stick_Right);

            box_EmulatedAxisList.Rows.Add("RIGHT STICK UP:", controller.EmulationMapping.Right_Stick_Up);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK DOWN:", controller.EmulationMapping.Right_Stick_Down);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK LEFT:", controller.EmulationMapping.Right_Stick_Left);
            box_EmulatedAxisList.Rows.Add("RIGHT STICK RIGHT:", controller.EmulationMapping.Right_Stick_Right);

            box_EmulatedAxisList.Rows.Add("DPAD UP:", controller.EmulationMapping.DPAD_UP);
            box_EmulatedAxisList.Rows.Add("DPAD DOWN:", controller.EmulationMapping.DPAD_DOWN);
            box_EmulatedAxisList.Rows.Add("DPAD LEFT:", controller.EmulationMapping.DPAD_LEFT);
            box_EmulatedAxisList.Rows.Add("DPAD RIGHT:", controller.EmulationMapping.DPAD_RIGHT);

            box_EmulatedAxisList.Rows.Add("LEFT TRIGGER:", controller.EmulationMapping.Left_Trigger);
            box_EmulatedAxisList.Rows.Add("RIGHT TRIGGER:", controller.EmulationMapping.Right_Trigger);
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
            IController controller = GetCurrentController();

            // Is XInput?
            bool isXInput = IsControllerXInput(controller);

            // Axis mapping entries.
            AxisMapping axisMappings = controller.AxisMapping;

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
            borderless_ControllerPort.Text = Convert.ToString(GetCurrentController().ControllerID);

            // Set "last" controller index.
            lastControllerIndex = borderless_CurrentController.SelectedIndex;

            // Set the new controller port.
            currentControllerPort = Convert.ToInt32(borderless_ControllerPort.Text);
        }

        /// <summary>
        /// Saves the currently selected controller configuration.
        /// </summary>
        private void SaveCurrentController()
        {
            // If there is a previous controller selection
            if (lastControllerIndex != -1)
            {
                // Get Controller
                IController currentController = controllerManager.Controllers[lastControllerIndex];

                // Set Only Controller Port, All Other Properties Were Already Saved when Set.
                currentController.ControllerID = Convert.ToInt32(borderless_ControllerPort.Text);

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
        private IController GetCurrentController()
        {   
            // Get current controller entry.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;
            return controllerManager.Controllers[currentControllerEntry];
        }

        /// <summary>
        /// Sets the new controller object for the currently selected controller.
        /// </summary>
        /// <param name="controller">The controller to replace the current controller with.</param>
        private void SetCurrentController(IController controller)
        {
            // Get current controller entry.
            int currentControllerEntry = borderless_CurrentController.SelectedIndex;

            // Assign current entry.
            controllerManager.Controllers[currentControllerEntry] = controller;
        }

        /// <summary>
        /// Returns true if the currently selected controller is an XInput Controller
        /// </summary>
        /// <param name="controller">The controller to check whether is or is not XInput</param>
        private bool IsControllerXInput(IController controller)
        {
            return controller.Remapper.DeviceType == Remapper.InputDeviceType.XInput ? true : false;
        }

        /// <summary>
        /// Retrieves an axis mapping entry for the specified controller and row
        /// of the axis assignment DataGridView.
        /// </summary>
        /// <returns>An axis mapping entry for the current row.</returns>        
        /// <param name="axisRow">Axis mapping DataGridView row for which to obtain the axis mapping.</param>
        /// <param name="controller">The controller object to get axis mapping from.</param>
        private AxisMappingEntry GetAxisMappingEntry(IController controller, AxisRows axisRow)
        {
            // Obtain axis mapping to modify.
            switch (axisRow)
            {
                case AxisRows.LeftStickX: return controller.AxisMapping.leftStickX; break;
                case AxisRows.LeftStickY: return controller.AxisMapping.leftStickY; break;
                case AxisRows.RightStickX: return controller.AxisMapping.rightStickX; break;
                case AxisRows.RightStickY: return controller.AxisMapping.rightStickY; break;
                case AxisRows.LeftTrigger: return controller.AxisMapping.leftTrigger; break;
                case AxisRows.RightTrigger: return controller.AxisMapping.rightTrigger; break;
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
            IController controller = GetCurrentController(); 

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
        private void ControllerButtonAssign(int row, IController controller, object dataGridView)
        {
            // Disable Mouse Tab Switching
            Global.BaseForm.EnableTabSwitching = false;

            // Get DatagridView Cell for Button
            DataGridView senderGridView = (DataGridView)dataGridView;
            DataGridViewCell sourceCell = senderGridView.Rows[(int)row].Cells[1];

            // Thread to update current assignment cell.
            Thread updateCellThread = new Thread ( () =>
            {
                while (true)
                {
                    box_AxisList.Invoke((Action)delegate { sourceCell.Value = CurrentTimeout.ToString("F2"); });
                    Thread.Sleep(16);
                }
            } );

            // Setup the button remap opertation.
            if (senderGridView == box_ButtonList) { SetupButtonRemap((ButtonRows)row, controller); }
            else if (senderGridView == box_EmulatedAxisList) { SetupButtonRemap((EmulationRows)row, controller); }
            else { return; }

            // Start tasks.
            updateCellThread.Start();
            controllerAssignTask.Start();

            // Wait for the task to finish.
            controllerAssignTask.Wait();
            updateCellThread.Abort();

            // Obtain task result (failed or succeeded remapping).
            bool successfulRemap = controllerAssignTask.Result;

            // Refresh curent button on successful remap, else assign 255.
            if (successfulRemap) {
                if (senderGridView == box_ButtonList)
                {
                    box_ButtonList.Invoke((Action)delegate { sourceCell.Value = GetButtonIndex((ButtonRows)row, controller); });
                }
                else if (senderGridView == box_EmulatedAxisList)
                {
                    box_ButtonList.Invoke((Action)delegate { sourceCell.Value = GetButtonIndex((EmulationRows)row, controller); });
                }
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
        private int GetButtonIndex(ButtonRows row, IController controller)
        {
            switch (row)
            {
                case ButtonRows.ButtonA: return controller.ButtonMapping.Button_A;
                case ButtonRows.ButtonB: return controller.ButtonMapping.Button_B;
                case ButtonRows.ButtonX: return controller.ButtonMapping.Button_X;
                case ButtonRows.ButtonY: return controller.ButtonMapping.Button_Y;
                case ButtonRows.ButtonLB: return controller.ButtonMapping.Button_LB;
                case ButtonRows.ButtonRB: return controller.ButtonMapping.Button_RB;
                case ButtonRows.ButtonLS: return controller.ButtonMapping.Button_LS;
                case ButtonRows.ButtonRS: return controller.ButtonMapping.Button_RS;
                case ButtonRows.ButtonBack: return controller.ButtonMapping.Button_Back;
                case ButtonRows.ButtonGuide: return controller.ButtonMapping.Button_Guide;
                case ButtonRows.ButtonStart: return controller.ButtonMapping.Button_Start;
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
        private int GetButtonIndex(EmulationRows row, IController controller)
        {
            switch (row)
            {
                case EmulationRows.LeftStickUp: return controller.EmulationMapping.Left_Stick_Up;
                case EmulationRows.LeftStickDown: return controller.EmulationMapping.Left_Stick_Down;
                case EmulationRows.LeftStickLeft: return controller.EmulationMapping.Left_Stick_Left;
                case EmulationRows.LeftStickRight: return controller.EmulationMapping.Left_Stick_Right;

                case EmulationRows.RightStickUp: return controller.EmulationMapping.Right_Stick_Up;
                case EmulationRows.RightStickDown: return controller.EmulationMapping.Right_Stick_Down;
                case EmulationRows.RightStickLeft: return controller.EmulationMapping.Right_Stick_Left;
                case EmulationRows.RightStickRight: return controller.EmulationMapping.Right_Stick_Right;

                case EmulationRows.DPadUp: return controller.EmulationMapping.DPAD_UP;
                case EmulationRows.DPadDown: return controller.EmulationMapping.DPAD_DOWN;
                case EmulationRows.DPadLeft: return controller.EmulationMapping.DPAD_LEFT;
                case EmulationRows.DPadRight: return controller.EmulationMapping.DPAD_RIGHT;

                case EmulationRows.LeftTrigger: return controller.EmulationMapping.Left_Trigger;
                case EmulationRows.RightTrigger: return controller.EmulationMapping.Right_Trigger;
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
        private void SetupButtonRemap(ButtonRows row, IController controller)
        {
            // Remap specified button
            switch (row)
            {
                case ButtonRows.ButtonA: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_A, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonB: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_B, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonX: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_X, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonY: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_Y, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonLB: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_LB, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonRB: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_RB, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonLS: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_LS, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonRS: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_RS, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonBack: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_Back, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonGuide: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_Guide, ref taskCancellationToken); }); break;
                case ButtonRows.ButtonStart: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.ButtonMapping.Button_Start, ref taskCancellationToken); }); break;
            }
        }

        /// <summary>
        /// Sets up the Controller Assign Task to perform a task of remapping the
        /// currently assigned button for X to the specified row of the emulation
        /// DataGridView
        /// </summary>
        /// <param name="row">The row of the datagridview for which the appropriate button should be remapped.</param>
        /// <param name="controller">The controller to remap button mapping from.</param>
        private void SetupButtonRemap(EmulationRows row, IController controller)
        {
            // Remap specified button
            switch (row)
            {
                case EmulationRows.LeftStickUp: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Left_Stick_Up, ref taskCancellationToken); }); break;
                case EmulationRows.LeftStickDown: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Left_Stick_Down, ref taskCancellationToken); }); break;
                case EmulationRows.LeftStickLeft: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Left_Stick_Left, ref taskCancellationToken); }); break;
                case EmulationRows.LeftStickRight: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Left_Stick_Right, ref taskCancellationToken); }); break;

                case EmulationRows.RightStickUp: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Right_Stick_Up, ref taskCancellationToken); }); break;
                case EmulationRows.RightStickDown: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Right_Stick_Down, ref taskCancellationToken); }); break;
                case EmulationRows.RightStickLeft: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Right_Stick_Left, ref taskCancellationToken); }); break;
                case EmulationRows.RightStickRight: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Right_Stick_Right, ref taskCancellationToken); }); break;

                case EmulationRows.DPadUp: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.DPAD_UP, ref taskCancellationToken); }); break;
                case EmulationRows.DPadDown: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.DPAD_DOWN, ref taskCancellationToken); }); break;
                case EmulationRows.DPadLeft: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.DPAD_LEFT, ref taskCancellationToken); }); break;
                case EmulationRows.DPadRight: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.DPAD_RIGHT, ref taskCancellationToken); }); break;

                case EmulationRows.LeftTrigger: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Left_Trigger, ref taskCancellationToken); }); break;
                case EmulationRows.RightTrigger: controllerAssignTask = new Task<bool>(() => { return controller.RemapButtons(REMAP_TIMEOUT, out CurrentTimeout, ref controller.EmulationMapping.Right_Trigger, ref taskCancellationToken); }); break;
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
                if (dataGridView == box_ButtonList) { DisableButtonRow((ButtonRows)currentRow); }
                else if (dataGridView == box_EmulatedAxisList) { DisableEmulatedRow((EmulationRows)currentRow); }
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
                case ButtonRows.ButtonA: GetCurrentController().ButtonMapping.Button_A = BUTTON_NULL; break;
                case ButtonRows.ButtonB: GetCurrentController().ButtonMapping.Button_B = BUTTON_NULL; break;
                case ButtonRows.ButtonX: GetCurrentController().ButtonMapping.Button_X = BUTTON_NULL; break;
                case ButtonRows.ButtonY: GetCurrentController().ButtonMapping.Button_Y = BUTTON_NULL; break;
                case ButtonRows.ButtonLB: GetCurrentController().ButtonMapping.Button_LB = BUTTON_NULL; break;
                case ButtonRows.ButtonRB: GetCurrentController().ButtonMapping.Button_RB = BUTTON_NULL; break;
                case ButtonRows.ButtonLS: GetCurrentController().ButtonMapping.Button_LS = BUTTON_NULL; break;
                case ButtonRows.ButtonRS: GetCurrentController().ButtonMapping.Button_RS = BUTTON_NULL; break;
                case ButtonRows.ButtonBack: GetCurrentController().ButtonMapping.Button_Back = BUTTON_NULL; break;
                case ButtonRows.ButtonGuide: GetCurrentController().ButtonMapping.Button_Guide = BUTTON_NULL; break;
                case ButtonRows.ButtonStart: GetCurrentController().ButtonMapping.Button_Start = BUTTON_NULL; break;
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
                case EmulationRows.DPadUp: GetCurrentController().EmulationMapping.DPAD_UP = BUTTON_NULL; break;
                case EmulationRows.DPadDown: GetCurrentController().EmulationMapping.DPAD_DOWN = BUTTON_NULL; break;
                case EmulationRows.DPadLeft: GetCurrentController().EmulationMapping.DPAD_LEFT = BUTTON_NULL; break;
                case EmulationRows.DPadRight: GetCurrentController().EmulationMapping.DPAD_RIGHT = BUTTON_NULL; break;

                case EmulationRows.LeftStickUp: GetCurrentController().EmulationMapping.Left_Stick_Up = BUTTON_NULL; break;
                case EmulationRows.LeftStickDown: GetCurrentController().EmulationMapping.Left_Stick_Down = BUTTON_NULL; break;
                case EmulationRows.LeftStickLeft: GetCurrentController().EmulationMapping.Left_Stick_Left = BUTTON_NULL; break;
                case EmulationRows.LeftStickRight: GetCurrentController().EmulationMapping.Left_Stick_Right = BUTTON_NULL; break;

                case EmulationRows.RightStickUp: GetCurrentController().EmulationMapping.Right_Stick_Up = BUTTON_NULL; break;
                case EmulationRows.RightStickDown: GetCurrentController().EmulationMapping.Right_Stick_Down = BUTTON_NULL; break;
                case EmulationRows.RightStickLeft: GetCurrentController().EmulationMapping.Right_Stick_Left = BUTTON_NULL; break;
                case EmulationRows.RightStickRight: GetCurrentController().EmulationMapping.Right_Stick_Right = BUTTON_NULL; break;

                case EmulationRows.LeftTrigger: GetCurrentController().EmulationMapping.Left_Trigger = BUTTON_NULL; break;
                case EmulationRows.RightTrigger: GetCurrentController().EmulationMapping.Right_Trigger = BUTTON_NULL; break;
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
            // Pass event to relevant column by index.
            switch (e.ColumnIndex)
            {
                // Remap Source Axis With Controller
                case (int)AxisColumns.AxisSource:

                    // Cancel current remapping.
                    CancelCurrentTask();

                    // Obtain controller object for remapping.
                    IController controller = GetCurrentController();

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
            if (controllerAssignTask != null)
            {
                taskCancellationToken = true;
                controllerAssignTask.Wait();
                taskCancellationToken = false;
            }
        }

        /// <summary>
        /// Polls the properties of the DirectInput controller and obtains a source axis.
        /// </summary>
        /// <param name="row">The row of whose source axis is to be changed.</param>
        /// <param name="controller">Specifies the current controller object.</param>
        private void ControllerSourceAxisAssign(AxisRows row, IController controller)
        {
            // Do not execute if the controller is XInput (XInput axis are pre-programmed).
            if (IsControllerXInput(controller)) { return; }

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
                    box_AxisList.Invoke((Action)delegate { sourceCell.Value = CurrentTimeout.ToString("F2"); });
                    Thread.Sleep(16);
                }
            } );

            // Start the thread
            // Remark: The thread will be stopped after the user either presses a key or after timeout.
            updateCellThread.Start();

            // Remapping Operation Commence.
            AxisMappingEntry axisMapping = GetAxisMappingEntry(controller, row);

            // Set Task
            controllerAssignTask = new Task<bool>(() => { return controller.RemapAxis(REMAP_TIMEOUT, out CurrentTimeout, axisMapping, ref taskCancellationToken); });

            // Start remapping polling task.
            controllerAssignTask.Start();

            // Wait for the task to finish.
            controllerAssignTask.Wait();
            updateCellThread.Abort();

            // Obtain task result (failed or succeeded remapping).
            bool successfulRemap = controllerAssignTask.Result;

            // Update if successfully remapped.
            if (successfulRemap) { sourceCell.Value = axisMapping.propertyName; } else { sourceCell.Value = "Null"; }
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
            ControllerAxis returnedAxis = (ControllerAxis)Enum.Parse(typeof(ControllerAxis), (string)deadzoneNumberDialog.GetValue());
            box_AxisList.Rows[(int)axisRow].Cells[(int)AxisColumns.AxisDestination].Value = returnedAxis;

            // Set value to axis mapping entry.
            IController currentController = GetCurrentController();
            AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.axis = returnedAxis;
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

            // Set value to axis mapping entry.
            IController currentController = GetCurrentController();
            AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.isReversed = isEnabled;
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
            IController currentController = GetCurrentController();
            AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.deadZone = deadZone;
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
            IController currentController = GetCurrentController();
            AxisMappingEntry axisMapping = GetAxisMappingEntry(currentController, axisRow);
            axisMapping.radiusScale = radius;
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
            controllerPollThread = new Thread
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
            controllerPollThread.Start();
        }

        /// <summary>
        /// Displays a preview of the currently applied controls to a specified controller port.
        /// </summary>
        private void ShowControllerPreview()
        {
            // Get the current set controller inputs for preview.
            ControllerInputs controllerInputs = controllerManager.GetInput(currentControllerPort);

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
        private void SetAnalogIndicatorValues(ControllerInputs controllerInputs)
        {
            // Retrieve analog stick X and Y values.
            float analogStickX = controllerInputs.leftStick.GetX();
            float analogStickY = controllerInputs.leftStick.GetY();
            float analogStickRX = controllerInputs.rightStick.GetX();
            float analogStickRY = controllerInputs.rightStick.GetY();

            // Scale analog values to controls.
            analogStickX = (analogStickX / ControllerCommon.AXIS_MAX_VALUE_F) * borderless_LeftStick.MAX_VALUE_RADIUS;
            analogStickY = (analogStickY / ControllerCommon.AXIS_MAX_VALUE_F) * borderless_LeftStick.MAX_VALUE_RADIUS;
            analogStickRX = (analogStickRX / ControllerCommon.AXIS_MAX_VALUE_F) * borderless_LeftStick.MAX_VALUE_RADIUS;
            analogStickRY = (analogStickRY / ControllerCommon.AXIS_MAX_VALUE_F) * borderless_LeftStick.MAX_VALUE_RADIUS;

            // Set analog sticks
            borderless_LeftStick.SetXYValue((int)analogStickX, (int)analogStickY);
            borderless_RightStick.SetXYValue((int)analogStickRX, (int)analogStickRY);
        }

        /// <summary>
        /// Sets the status of the trigger pressure indicators on the preview section
        /// of the input window. Moves the bar height appropriately in response to the realtime
        /// axis changes.
        /// </summary>
        /// <param name="controllerInputs">The structure containing the current controller inputs.</param>
        private void SetTriggers(ControllerInputs controllerInputs)
        {
            // Retrieve trigger pressure values.
            float leftTriggerPressure = controllerInputs.GetLeftTriggerPressure();
            float rightTriggerPressure = controllerInputs.GetRightTriggerPressure();

            // Scale trigger pressure values to controls.
            leftTriggerPressure = (leftTriggerPressure / ControllerCommon.AXIS_MAX_VALUE_F) * borderless_LeftStick.MAX_VALUE_RADIUS;
            rightTriggerPressure = (rightTriggerPressure / ControllerCommon.AXIS_MAX_VALUE_F) * borderless_LeftStick.MAX_VALUE_RADIUS;

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
        private void SetButtons(ControllerInputs controllerInputs)
        {
            // Set button press states for each button.
            borderless_ButtonA.ButtonEnabled = controllerInputs.controllerButtons.Button_A;
            borderless_ButtonB.ButtonEnabled = controllerInputs.controllerButtons.Button_B;
            borderless_ButtonX.ButtonEnabled = controllerInputs.controllerButtons.Button_X;
            borderless_ButtonY.ButtonEnabled = controllerInputs.controllerButtons.Button_Y;
            borderless_ButtonL.ButtonEnabled = controllerInputs.controllerButtons.Button_LB;
            borderless_ButtonR.ButtonEnabled = controllerInputs.controllerButtons.Button_RB;

            borderless_ButtonUP.ButtonEnabled = controllerInputs.controllerButtons.DPAD_UP;
            borderless_ButtonDOWN.ButtonEnabled = controllerInputs.controllerButtons.DPAD_DOWN;
            borderless_ButtonLEFT.ButtonEnabled = controllerInputs.controllerButtons.DPAD_LEFT;
            borderless_ButtonRIGHT.ButtonEnabled = controllerInputs.controllerButtons.DPAD_RIGHT;
            borderless_ButtonSELECT.ButtonEnabled = controllerInputs.controllerButtons.Button_Back;
            borderless_ButtonSTART.ButtonEnabled = controllerInputs.controllerButtons.Button_Start;
        }
        #endregion Controller Input Preview

        /// <summary>
        /// When the user presses the keyboard keys, 
        /// allow only numeric keypresses.
        /// </summary>
        private void ControllerPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the number is not a digit or control character (e.g. backspace) do nothing with it.
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Ensure that the digit is a number when the user leaves the textbox.
        /// </summary>
        private void ControllerPort_Leave(object sender, EventArgs e)
        {
            // Check if string is numeric by parse attempt.
            var isNumeric = int.TryParse(borderless_ControllerPort.Text, out int n);

            // If it's not numeric, reset to 0.
            if (! isNumeric) { borderless_ControllerPort.Text = "0"; }

            // Set the new controller port.
            currentControllerPort = Convert.ToInt32(borderless_ControllerPort.Text);

            // Get Current Controller
            IController currentController = controllerManager.Controllers[lastControllerIndex];

            // Set Current Controller Port
            currentController.ControllerID = Convert.ToInt32(borderless_ControllerPort.Text);
        }
    }
}
