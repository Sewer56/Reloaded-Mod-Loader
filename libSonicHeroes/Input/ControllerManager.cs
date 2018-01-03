using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonicHeroes.Input.DirectInput;
using SonicHeroes.Input.XInput;
using static SonicHeroes.Input.ControllerCommon;
using static SonicHeroes.Input.Hotplugger;

namespace SonicHeroes.Input
{
    /// <summary>
    /// Provides the management of XInput and DInput controllers, loading all of the controllers and receiving + handling input for each controller.
    /// </summary>
    class ControllerManager
    {
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
        /// Initializes the controller manager which loads all of the DInput and XInput controllers and 
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
        /// Sets up the DirectInput controllers and Initializes the XInput Controllers.
        /// Basically instantiates all of the controllers and sets them up.
        /// </summary>
        public void SetupControllerManager()
        {
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
        }

        /// <summary>
        /// Retrieves the inputs for a specific controller port.
        /// </summary>
        /// <param name="controllerPort">The port of the controller. Starting with port 0.</param>
        public Controller_Inputs GetInput(int controllerPort)
        {
            // Retrieve all controllers at port #.
            List<IController> controllersAtPort = Controllers.Where(x => x.ControllerID == controllerPort).ToList();

            // Get input for every controller at port # and add onto the input struct.
            Controller_Inputs controllerInputs = new Controller_Inputs();
            controllerInputs.controllerButtons = new Controller_Button_Struct();
            controllerInputs.leftStick = new Analog_Stick();

            // For each controller in port #.
            foreach (IController controller in controllersAtPort)
            {
                // Get inputs for the controller.
                Controller_Inputs controllerInputsNew = controller.GetControllerState();

                // Add onto Left stick and Right Stick
                controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetX() + controllerInputsNew.leftStick.GetX());
                controllerInputs.leftStick.SetY(controllerInputs.leftStick.GetY() + controllerInputsNew.leftStick.GetY());
                controllerInputs.rightStick.SetX(controllerInputs.rightStick.GetX() + controllerInputsNew.rightStick.GetX());
                controllerInputs.rightStick.SetY(controllerInputs.rightStick.GetY() + controllerInputsNew.rightStick.GetY());

                // Add triggers.
                controllerInputs.SetLeftTriggerPressure(controllerInputs.GetLeftTriggerPressure() + controllerInputsNew.GetLeftTriggerPressure());
                controllerInputs.SetRightTriggerPressure(controllerInputs.GetRightTriggerPressure() + controllerInputsNew.GetRightTriggerPressure());

                // Add DPAD
                if (controllerInputsNew.controllerButtons.DPAD_UP) { controllerInputs.controllerButtons.DPAD_UP = true; }
                if (controllerInputsNew.controllerButtons.DPAD_LEFT) { controllerInputs.controllerButtons.DPAD_LEFT = true; }
                if (controllerInputsNew.controllerButtons.DPAD_DOWN) { controllerInputs.controllerButtons.DPAD_DOWN = true; }
                if (controllerInputsNew.controllerButtons.DPAD_RIGHT) { controllerInputs.controllerButtons.DPAD_RIGHT = true; }

                // Add buttons.
                if (controllerInputsNew.controllerButtons.Button_A) { controllerInputs.controllerButtons.Button_A = true; }
                if (controllerInputsNew.controllerButtons.Button_B) { controllerInputs.controllerButtons.Button_B = true; }
                if (controllerInputsNew.controllerButtons.Button_X) { controllerInputs.controllerButtons.Button_X = true; }
                if (controllerInputsNew.controllerButtons.Button_Y) { controllerInputs.controllerButtons.Button_Y = true; }

                if (controllerInputsNew.controllerButtons.Button_LB) { controllerInputs.controllerButtons.Button_LB = true; }
                if (controllerInputsNew.controllerButtons.Button_RB) { controllerInputs.controllerButtons.Button_RB = true; }
                if (controllerInputsNew.controllerButtons.Button_LS) { controllerInputs.controllerButtons.Button_LS = true; }
                if (controllerInputsNew.controllerButtons.Button_RS) { controllerInputs.controllerButtons.Button_RS = true; }

                if (controllerInputsNew.controllerButtons.Button_Back) { controllerInputs.controllerButtons.Button_Back = true; }
                if (controllerInputsNew.controllerButtons.Button_Guide) { controllerInputs.controllerButtons.Button_Guide = true; }
                if (controllerInputsNew.controllerButtons.Button_Start) { controllerInputs.controllerButtons.Button_Start = true; }
            }

            // Return port state.
            return controllerInputs;

        }
    }
}
