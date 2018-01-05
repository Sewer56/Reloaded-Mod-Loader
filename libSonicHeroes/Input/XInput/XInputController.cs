using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using SonicHeroes.Input;
using static SonicHeroes.Input.DirectInput.DInputCommon;
using static SonicHeroes.Input.ControllerCommon;
using SonicHeroes.Input.DirectInput;

namespace SonicHeroes.Input.XInput
{
    /// <summary>
    /// Wrapper class that creates an instnace of an XInput controller and outputs it in a format more similar to DirectInput.
    /// Provides functionality for obtaining input from an XInput controller within the mod loader.
    /// This is a somewhat thin wrapper of the current DInput implementation.
    /// </summary>
    public class XInputController : IController
    {
        /// <summary>
        /// Defines the maximum value returnable by an XInput analog stick.
        /// </summary>
        public const float MAX_ANALOG_STICK_RANGE_XINPUT = 32767;

        /// <summary>
        /// Defines the maximum value returnable by an XInput trigger.
        /// </summary>
        public const float MAX_TRIGGER_RANGE_XINPUT = 255;

        /// <summary>
        /// Store the individual button mappings structure for this controller.
        /// </summary>
        public ButtonMapping ButtonMapping { get; set; }

        /// <summary>
        /// Store the individual axis mappings structure for this controller.
        /// </summary>
        public AxisMapping AxisMapping { get; set; }

        /// <summary>
        /// Defines the custom botton mapping which simulates the individual axis and analog inputs.
        /// </summary>
        public EmulationButtonMapping EmulationMapping { get; set; }

        /// <summary>
        /// Defines the individual port used for this specific controller.
        /// </summary>
        public int ControllerID { get; set; }

        /// <summary>
        /// Holds the current instance of the XImput Controller.
        /// </summary>
        public SharpDX.XInput.Controller Controller { get; set; }

        /// <summary>
        /// Holds the current instance of the XImput Controller.
        /// </summary>
        private SharpDX.XInput.State ControllerState { get; set; }

        /// <summary>
        /// Provides a control scheme remapper allowing for buttons to be remapped on the fly.
        /// </summary>
        public Remapper Remapper { get; set; }

        /// <summary>
        /// Constructor for this class, defines the individual controller.
        /// </summary>
        public XInputController(int controllerID)
        {
            // Create controller instance and assign controller port.
            Controller = new SharpDX.XInput.Controller((UserIndex)controllerID);
            ControllerID = controllerID;

            // Instantiate the remapper.
            Remapper = new Remapper(Remapper.InputDeviceType.DirectInput, this);

            // Get the controller key binding.
            Remapper.GetMappings();
        }

        /// <summary>
        /// Waits for the user to move an axis and retrieves the last pressed axis. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
        public void RemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry) { Remapper.RemapAxis(timeoutSeconds, out currentTimeout, mappingEntry); }

        /// <summary>
        /// Waits for the user to press a button and retrieves the last pressed button. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        public void RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap) { Remapper.RemapButtons(timeoutSeconds, out currentTimeout, ref buttonToMap); }

        /// <summary>
        /// Retrieves whether a specific button is pressed or not. 
        /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
        /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
        /// True if said button is pressed, else false.
        /// </summary>
        public bool GetButtonState(Controller_Buttons_Generic button)
        {
            // Retrieve current button state.
            bool[] buttons = GetButtons();

            // Retrieve requested button index.
            int buttonIndex = DInputGetMappedButtonIndex(button, ButtonMapping);

            // Return the state declaring if the joystick button is pressed.
            return buttons[buttonIndex];
        }

        /// <summary>
        /// Retrieves the specific intensity in terms of how far/deep an axis is pressed in.
        /// The return value should be a floating point number between -100 and 100 float.
        /// For triggers, the range is a value between 0 and 100.
        /// </summary>
        public float GetAxisState(ControllerAxis axis)
        {
            // Retrieve requested axis mapping entry.
            AxisMappingEntry controllerAxisMapping = DInputGetMappedAxis(axis, AxisMapping);

            // Retrieve the intensity of the axis press-in value.
            return XInputGetAxisValue(controllerAxisMapping);
        }

        /// <summary>
        /// Returns the current value for a given axis based off of the set axis in the axis mapping entry and the joystick state.
        /// Performs any necessary additional operations on the axis values based off of the axis configuration.
        /// </summary>
        /// <param name="mappingEntry">The mapping entry for the axis defining which axis should be used.</param>
        public float XInputGetAxisValue(AxisMappingEntry mappingEntry)
        {
            // Obtain the raw scaled value (to match XInput range) for the requested axis.
            int rawValue = GetAxisRawScaledValue(mappingEntry);

            // Process the raw axis value and return.
            return DInputProcessAxisRawValue(rawValue, mappingEntry);
        }

        /// <summary>
        /// Updates the current state of the controller in question, retrieving the current button presses
        /// and axis measurements.
        /// </summary>
        public void UpdateControllerState()
        {
            ControllerState = Controller.GetState();
        }

        /// <summary>
        /// Returns the currently pressed buttons on the XInput controller as an array of buttons.
        /// </summary>
        public bool[] GetButtons()
        {
            // Create the array of buttons.
            bool[] buttons = new bool[Enum.GetNames(typeof(Controller_Buttons_Generic)).Length];

            // Get XBOX Buttons
            GamepadButtonFlags buttonFlags = ControllerState.Gamepad.Buttons;

            // Retrieve Controller Button Status.
            buttons[(int)Controller_Buttons_Generic.Button_A] = (buttonFlags.HasFlag(GamepadButtonFlags.A)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_B] = (buttonFlags.HasFlag(GamepadButtonFlags.B)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_X] = (buttonFlags.HasFlag(GamepadButtonFlags.X)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_Y] = (buttonFlags.HasFlag(GamepadButtonFlags.Y)) ? true : false;

            buttons[(int)Controller_Buttons_Generic.Button_LS] = (buttonFlags.HasFlag(GamepadButtonFlags.LeftThumb)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_RS] = (buttonFlags.HasFlag(GamepadButtonFlags.RightThumb)) ? true : false;

            buttons[(int)Controller_Buttons_Generic.Button_LB] = (buttonFlags.HasFlag(GamepadButtonFlags.LeftShoulder)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_RB] = (buttonFlags.HasFlag(GamepadButtonFlags.RightShoulder)) ? true : false;

            buttons[(int)Controller_Buttons_Generic.Button_Back] = (buttonFlags.HasFlag(GamepadButtonFlags.Back)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_Start] = (buttonFlags.HasFlag(GamepadButtonFlags.Start)) ? true : false;
            buttons[(int)Controller_Buttons_Generic.Button_Guide] = false;

            // Return buttons.
            return buttons;
        }

        /// <summary>
        /// Retrieves the state of all of the axis and buttons of the controller as well as the DPAD state
        /// and retrieves it in a struct format convenient for the modder's use.
        /// </summary>
        /// <returns>Controller inputs as a custom struct.</returns>
        public ControllerInputs GetControllerState()
        {
            // Update the current state of the Joystick/Controller
            ControllerState = Controller.GetState();

            // Instantiate an instance of controller inputs.
            ControllerInputs controllerInputs = new ControllerInputs();

            // Check if controller is connected.
            if (Controller.IsConnected)
            {
                // Retrieve all of the buttons;
                controllerInputs = GetControllerStateButtons(controllerInputs);

                // Retrieve all of the axis.
                controllerInputs = GetControllerStateAxis(controllerInputs);

                // Retrieve DPAD Information
                GamepadButtonFlags buttonFlags = ControllerState.Gamepad.Buttons;

                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadUp)) { controllerInputs.controllerButtons.DPAD_UP = true; }
                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadLeft)) { controllerInputs.controllerButtons.DPAD_LEFT = true; }
                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadRight)) { controllerInputs.controllerButtons.DPAD_RIGHT = true; }
                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadDown)) { controllerInputs.controllerButtons.DPAD_DOWN = true; }

                // Retrieve Emulated Keys
                controllerInputs = GetControllerState_EmulatedKeys(controllerInputs, GetButtons());
            }

            // Return to base.
            return controllerInputs;
        }

        /// <summary>
        /// Retrieves and returns the state of all of the controller's individual buttons.
        /// </summary>
        /// <param name="controllerInputs">The controller input struct.</param>
        private ControllerInputs GetControllerStateButtons(ControllerInputs controllerInputs)
        {
            // Retrieve all of the buttons.
            controllerInputs.controllerButtons.Button_A = GetButtonState(Controller_Buttons_Generic.Button_A);
            controllerInputs.controllerButtons.Button_B = GetButtonState(Controller_Buttons_Generic.Button_B);
            controllerInputs.controllerButtons.Button_X = GetButtonState(Controller_Buttons_Generic.Button_X);
            controllerInputs.controllerButtons.Button_Y = GetButtonState(Controller_Buttons_Generic.Button_Y);

            controllerInputs.controllerButtons.Button_LB = GetButtonState(Controller_Buttons_Generic.Button_LB);
            controllerInputs.controllerButtons.Button_RB = GetButtonState(Controller_Buttons_Generic.Button_RB);

            controllerInputs.controllerButtons.Button_Back = GetButtonState(Controller_Buttons_Generic.Button_Back);
            controllerInputs.controllerButtons.Button_Guide = GetButtonState(Controller_Buttons_Generic.Button_Guide);
            controllerInputs.controllerButtons.Button_Start = GetButtonState(Controller_Buttons_Generic.Button_Start);

            // Return the buttons.
            return controllerInputs;
        }

        /// <summary>
        /// Retrieves and returns the state of all of the controller's individual buttons.
        /// </summary>
        /// <param name="controllerInputs">The controller input struct.</param>
        private ControllerInputs GetControllerStateAxis(ControllerInputs controllerInputs)
        {
            // Retrieve all of the axis.
            controllerInputs.SetLeftTriggerPressure(GetAxisState(ControllerAxis.Left_Trigger_Pressure));
            controllerInputs.SetRightTriggerPressure(GetAxisState(ControllerAxis.Right_Trigger_Pressure));

            controllerInputs.leftStick.SetX(GetAxisState(ControllerAxis.Left_Stick_X));
            controllerInputs.leftStick.SetY(GetAxisState(ControllerAxis.Left_Stick_Y));

            controllerInputs.rightStick.SetX(GetAxisState(ControllerAxis.Right_Stick_X));
            controllerInputs.rightStick.SetY(GetAxisState(ControllerAxis.Right_Stick_Y));

            // Return the axis.
            return controllerInputs;
        }

        /// <summary>
        /// Retrieves the raw value for a specified passed in requested axis in integer form.
        /// </summary>
        /// <param name="mappingEntry">The specific axis mapping entry that is to be used.</param>
        private int GetAxisRawScaledValue(AxisMappingEntry mappingEntry)
        {
            // Value for the current axis.
            int rawValue = 0;

            // Return the appropriately mapped axis!
            switch (mappingEntry.axis)
            {
                case ControllerAxis.Left_Stick_X: rawValue = ControllerState.Gamepad.LeftThumbX; break;
                case ControllerAxis.Left_Stick_Y: rawValue = ControllerState.Gamepad.LeftThumbY; break;
                case ControllerAxis.Right_Stick_X: rawValue = ControllerState.Gamepad.RightThumbX; break;
                case ControllerAxis.Right_Stick_Y: rawValue = ControllerState.Gamepad.RightThumbY; break;
                case ControllerAxis.Left_Trigger_Pressure: rawValue = ControllerState.Gamepad.LeftTrigger; break;
                case ControllerAxis.Right_Trigger_Pressure: rawValue = ControllerState.Gamepad.RightTrigger; break;
                default: break;
            }

            // Process the value to DINput Ranges
            switch (mappingEntry.axis)
            {
                case ControllerAxis.Left_Stick_X: 
                case ControllerAxis.Left_Stick_Y: 
                case ControllerAxis.Right_Stick_X:
                case ControllerAxis.Right_Stick_Y:
                    // Scale from -32768-32767 to -100-100
                    rawValue = (int)( (rawValue / MAX_ANALOG_STICK_RANGE_XINPUT) * DInputManager.AXIS_MAX_VALUE_F); 
                    break;
                case ControllerAxis.Left_Trigger_Pressure: 
                case ControllerAxis.Right_Trigger_Pressure:
                    rawValue = ControllerState.Gamepad.RightTrigger;
                    
                    // Scale from 0-255 to 0-200
                    rawValue = (int)((rawValue / MAX_TRIGGER_RANGE_XINPUT) * DInputManager.AXIS_MAX_VALUE_F * (1.0F / DInputManager.TRIGGER_SCALE_FACTOR));

                    // Scale to -100-100 to simulate DInput.
                    rawValue -= 100;
                    break;
                default: break;
            }

            // Return the raw value.
            return rawValue;
        }

        /// <summary>
        /// Retrieves and returns the state of all of the inputs that are emulated on a digital (keyboard)
        /// to analog basis to allow the use of e.g. analog sticks within keyboards.
        /// If a non-zero value is set and the button is not pressed, the original value will not be overwritten.
        /// (i.e. Only overrides if input is sent)
        /// </summary>
        /// <returns></returns>
        private ControllerInputs GetControllerState_EmulatedKeys(ControllerInputs controllerInputs, bool[] buttons)
        {
            // Retrieve Emulated DPAD Keys
            #region DPAD Keys
            if (EmulationMapping.DPAD_DOWN != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_DOWN, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_DOWN = true; }
            }
            if (EmulationMapping.DPAD_LEFT != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_LEFT, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_LEFT = true; }
            }
            if (EmulationMapping.DPAD_RIGHT != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_RIGHT, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_RIGHT = true; }
            }
            if (EmulationMapping.DPAD_UP != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_UP, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_UP = true; }
            }
            #endregion

            // Retrieve Emulated Left Analog Stick
            #region Left Analog Stick
            if (EmulationMapping.Left_Stick_Down != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Down, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetY() + DInputManager.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Left_Stick_Left != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Left, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetX() + DInputManager.AXIS_MIN_VALUE_F); }
            }

            if (EmulationMapping.Left_Stick_Right != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Right, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetX() + DInputManager.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Left_Stick_Up != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Up, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetY() + DInputManager.AXIS_MIN_VALUE_F); }
            }
            #endregion

            // Retrieve Emulated Right Analog Stick
            #region Right Analog Stick
            if (EmulationMapping.Right_Stick_Down != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Down, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.rightStick.GetY() + DInputManager.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Right_Stick_Left != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Left, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.rightStick.GetX() + DInputManager.AXIS_MIN_VALUE_F); }
            }

            if (EmulationMapping.Right_Stick_Right != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Right, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.rightStick.GetX() + DInputManager.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Right_Stick_Up != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Up, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.rightStick.GetY() + DInputManager.AXIS_MIN_VALUE_F); }
            }
            #endregion

            // Retrieve Emulated Triggers
            #region Triggers
            if (EmulationMapping.Right_Trigger != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Trigger, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.SetRightTriggerPressure(DInputManager.AXIS_MAX_VALUE_F * DInputManager.TRIGGER_SCALE_FACTOR); }
            }

            if (EmulationMapping.Left_Trigger != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Trigger, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.SetLeftTriggerPressure(DInputManager.AXIS_MAX_VALUE_F / DInputManager.TRIGGER_SCALE_FACTOR); }
            }
            #endregion

            // Return controller inputs
            return controllerInputs;
        }
    }
}
