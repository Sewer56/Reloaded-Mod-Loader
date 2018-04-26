/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
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

using System.Collections.Generic;
using System.Linq;
using Reloaded.Input.DirectInput;
using Reloaded.Input.Modules;
using Reloaded.Input.XInput;
using static Reloaded.Input.Common.ControllerCommon;
using static Reloaded.Input.Modules.Hotplugger;

namespace Reloaded.Input
{
    /// <summary>
    /// Provides the management of XInput and DInput controllers, loading all of the controllers and receiving + handling input for each controller.
    /// </summary>
    public class ControllerManager
    {
        /// <summary>
        /// Delegate for controller hotplugging which gets fired when a controller is attached or detached
        /// from the currently connected controllers. Fires after the new controllers are acquired.
        /// </summary>
        public delegate void ControllerHotplugDelegate();

        /// <summary>
        /// Initializes the controller manager which loads all of the DInput and XInput controllers and 
        /// allows for quick and easy remapping and input obtaining from controllers.
        /// </summary>
        public ControllerManager()
        {
            // Instantiate controller list and DInputManager as well as Controller List.
            DInputManager = new DInputManager();
            Controllers = new List<IController>();

            // Set up the hotplugger instance.
            Hotplugger = new Hotplugger((GetConnectedControllersDelegate)SetupControllerManager);

            // Setup DInput Controllers and Instantiate XInput Controllers
            SetupControllerManager();
        }

        /// <summary>
        /// Manages all currently connected DirectInput devices.
        /// </summary>
        public DInputManager DInputManager { get; set; }

        /// <summary>
        /// Stores the list of all currently connected controllers, both XInput and DInput.
        /// </summary>
        public List<IController> Controllers { get; set; }

        /// <summary>
        /// Hotplugger which intercepts controller connections and disconnections and reacquires all controllers.
        /// </summary>
        public Hotplugger Hotplugger { get; set; }

        /// <summary>
        /// Delegate for controller hotplugging which gets fired when a controller is attached or detached
        /// from the currently connected controllers. Fires after the new controllers are acquired.
        /// </summary>
        public ControllerHotplugDelegate ControllerHotplugEventDelegate { get; set; }

        /// <summary>
        /// Sets up the DirectInput controllers and Initializes the XInput Controllers.
        /// Basically instantiates all of the controllers and sets them up.
        /// </summary>
        public void SetupControllerManager()
        {
            // Clear current controller (on hotplug events)
            Controllers.Clear();

            // Acquire the DirectInput devices. (Finds, loads and sets up all DInput Devices)
            DInputManager.AcquireDevices();

            // Instantiate three XInput Controllers
            XInputController xInputController1 = new XInputController(0);
            XInputController xInputController2 = new XInputController(1);
            XInputController xInputController3 = new XInputController(2);
            XInputController xInputController4 = new XInputController(3);

            // Place all controllers in Controllers list.
            Controllers.AddRange(DInputManager.RetrieveDevices());
            Controllers.Add(xInputController1);
            Controllers.Add(xInputController2);
            Controllers.Add(xInputController3);
            Controllers.Add(xInputController4);

            // Fire hotplug delegate.
            ControllerHotplugEventDelegate?.Invoke();
        }

        /// <summary>
        /// Retrieves the inputs for a specific controller port.
        /// </summary>
        /// <param name="controllerPort">The port of the controller. Starting with port 0.</param>
        public ControllerInputs GetInput(int controllerPort)
        {
            // Retrieve all controllers at port #.
            List<IController> controllersAtPort = Controllers.Where(x => x.InputMappings.ControllerId == controllerPort).ToList();

            // Get input for every controller at port # and add onto the input struct.
            ControllerInputs controllerInputs = new ControllerInputs
            {
                ControllerButtons = new ControllerButtonStruct(),
                LeftStick = new AnalogStick()
            };

            // For each controller in port #.
            foreach (IController controller in controllersAtPort)
            {
                // Get inputs for the controller.
                ControllerInputs controllerInputsNew = controller.GetControllerState();

                // Add onto Left stick and Right Stick
                controllerInputs.LeftStick.SetX(controllerInputs.LeftStick.GetX() + controllerInputsNew.LeftStick.GetX());
                controllerInputs.LeftStick.SetY(controllerInputs.LeftStick.GetY() + controllerInputsNew.LeftStick.GetY());
                controllerInputs.RightStick.SetX(controllerInputs.RightStick.GetX() + controllerInputsNew.RightStick.GetX());
                controllerInputs.RightStick.SetY(controllerInputs.RightStick.GetY() + controllerInputsNew.RightStick.GetY());

                // Add triggers.
                controllerInputs.SetLeftTriggerPressure(controllerInputs.GetLeftTriggerPressure() + controllerInputsNew.GetLeftTriggerPressure());
                controllerInputs.SetRightTriggerPressure(controllerInputs.GetRightTriggerPressure() + controllerInputsNew.GetRightTriggerPressure());

                // Add DPAD
                if (controllerInputsNew.ControllerButtons.DpadUp) controllerInputs.ControllerButtons.DpadUp = true;
                if (controllerInputsNew.ControllerButtons.DpadLeft) controllerInputs.ControllerButtons.DpadLeft = true;
                if (controllerInputsNew.ControllerButtons.DpadDown) controllerInputs.ControllerButtons.DpadDown = true;
                if (controllerInputsNew.ControllerButtons.DpadRight) controllerInputs.ControllerButtons.DpadRight = true;

                // Add buttons.
                if (controllerInputsNew.ControllerButtons.ButtonA) controllerInputs.ControllerButtons.ButtonA = true;
                if (controllerInputsNew.ControllerButtons.ButtonB) controllerInputs.ControllerButtons.ButtonB = true;
                if (controllerInputsNew.ControllerButtons.ButtonX) controllerInputs.ControllerButtons.ButtonX = true;
                if (controllerInputsNew.ControllerButtons.ButtonY) controllerInputs.ControllerButtons.ButtonY = true;

                if (controllerInputsNew.ControllerButtons.ButtonLb) controllerInputs.ControllerButtons.ButtonLb = true;
                if (controllerInputsNew.ControllerButtons.ButtonRb) controllerInputs.ControllerButtons.ButtonRb = true;
                if (controllerInputsNew.ControllerButtons.ButtonLs) controllerInputs.ControllerButtons.ButtonLs = true;
                if (controllerInputsNew.ControllerButtons.ButtonRs) controllerInputs.ControllerButtons.ButtonRs = true;

                if (controllerInputsNew.ControllerButtons.ButtonBack) controllerInputs.ControllerButtons.ButtonBack = true;
                if (controllerInputsNew.ControllerButtons.ButtonGuide) controllerInputs.ControllerButtons.ButtonGuide = true;
                if (controllerInputsNew.ControllerButtons.ButtonStart) controllerInputs.ControllerButtons.ButtonStart = true;
            }

            // Return port state.
            return controllerInputs;

        }
    }
}
