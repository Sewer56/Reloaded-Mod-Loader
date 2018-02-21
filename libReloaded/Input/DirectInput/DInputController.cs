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

using SharpDX.DirectInput;
using System;
using static Reloaded.Input.ControllerCommon;
using static Reloaded.Input.DirectInput.DInputCommon;

namespace Reloaded.Input.DirectInput
{
    /// <summary>
    /// Provides functionality for obtaining input from a DirectInput controller within the mod loader.
    /// Extended to allow for keyboard functionality.
    /// </summary>
    public class DInputController : Joystick, IController
    {
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
        /// Defines the current state of the DirectInput controller such as the pressed buttons, etc.
        /// </summary>
        public JoystickState JoystickState { get; set; }

        /// <summary>
        /// Provides a control scheme remapper allowing for buttons to be remapped on the fly.
        /// </summary>
        public Remapper Remapper { get; set; }

        /// <summary>
        /// Constructor for this class, defines the individual controller.
        /// </summary>
        public DInputController(SharpDX.DirectInput.DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {
            // Instantiate the remapper.
            Remapper = new Remapper(Remapper.InputDeviceType.DirectInput, this);

            // Get the controller key binding.
            Remapper.GetMappings();
        }

        /// <summary>
        /// Updates the current state of the controller in question, retrieving the current button presses
        /// and axis measurements.
        /// </summary>
        public void UpdateControllerState()
        {
            JoystickState = this.GetCurrentState();
        }

        /// <summary>
        /// Waits for the user to move an axis and retrieves the last pressed axis. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new axis has been successfully assigned.</returns>
        public bool RemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry, ref bool cancellationToken) { return Remapper.RemapAxis(timeoutSeconds, out currentTimeout, mappingEntry, ref cancellationToken); }

        /// <summary>
        /// Waits for the user to press a button and retrieves the last pressed button. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new button has been successfully assigned.</returns>
        public bool RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken) { return Remapper.RemapButtons(timeoutSeconds, out currentTimeout, ref buttonToMap, ref cancellationToken); }

        /// <summary>
        /// Retrieves whether a specific button is pressed or not. 
        /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
        /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
        /// True if said button is pressed, else false.
        /// </summary>
        public bool GetButtonState(Controller_Buttons_Generic button)
        {
            // Retrieve requested button index.
            int buttonIndex = DInputGetMappedButtonIndex(button, ButtonMapping);

            // Return the state declaring if the joystick button is pressed.
            return buttonIndex == 255 ? false : JoystickState.Buttons[buttonIndex];
        }

        /// <summary>
        /// Retrieves the specific intensity in terms of how far/deep an axis is pressed in.
        /// The return value should be a floating point number between -100 and 100 float.
        /// For triggers, the range is a value between 0 and 100.
        /// </summary>
        /// <remarks>
        /// This does not take into account the destination axis and reads the value
        /// of the equivalent source axis. If the user has Left Stick mapped to e.g. Right Stick
        /// and you request the right stick axis, the value will return 0 (assuming right stick is centered).
        /// </remarks>
        public float GetAxisState(ControllerAxis axis)
        {
            // Retrieve requested axis mapping entry.
            AxisMappingEntry controllerAxisMapping = InputGetMappedAxis(axis, AxisMapping);

            // Retrieve the intensity of the axis press-in value.
            return DInputGetAxisValue(controllerAxisMapping, JoystickState);
        }

        /// <summary>
        /// Retrieves all of the individual button states as an array of boolean values.
        /// True if a button is pressed, false if a button is not pressed.
        /// Note: The current controller state must first be manually updated.
        /// </summary>
        /// <returns></returns>
        public bool[] GetButtons()
        {
            return JoystickState.Buttons;
        }

        /// <summary>
        /// Retrieves the state of all of the axis and buttons of the controller as well as the DPAD state
        /// and retrieves it in a struct format convenient for the modder's use.
        /// </summary>
        /// <returns>Controller inputs as a custom struct.</returns>
        public ControllerInputs GetControllerState()
        {
            // Update the current state of the Joystick/Controller
            JoystickState = this.GetCurrentState();

            // Instantiate an instance of controller inputs.
            ControllerInputs controllerInputs = new ControllerInputs();

            // Retrieve all of the buttons;
            controllerInputs = GetCurrentButtons(controllerInputs);

            // Retrieve all of the axis.
            controllerInputs = GetCurrentAxis(controllerInputs);

            // Retrieve DPAD Information
            if (JoystickState.PointOfViewControllers[0] == -1) { }
            else
            {
                switch ((DPAD_Direction)JoystickState.PointOfViewControllers[0])
                {
                    case DPAD_Direction.UP: controllerInputs.controllerButtons.DPAD_UP = true; break;
                    case DPAD_Direction.DOWN: controllerInputs.controllerButtons.DPAD_DOWN = true; break;
                    case DPAD_Direction.LEFT: controllerInputs.controllerButtons.DPAD_LEFT = true; break;
                    case DPAD_Direction.RIGHT: controllerInputs.controllerButtons.DPAD_RIGHT = true; break;
                    case DPAD_Direction.UP_LEFT: controllerInputs.controllerButtons.DPAD_UP = true; controllerInputs.controllerButtons.DPAD_LEFT = true; break;
                    case DPAD_Direction.UP_RIGHT: controllerInputs.controllerButtons.DPAD_UP = true; controllerInputs.controllerButtons.DPAD_RIGHT = true; break;
                    case DPAD_Direction.DOWN_LEFT: controllerInputs.controllerButtons.DPAD_DOWN = true; controllerInputs.controllerButtons.DPAD_LEFT = true; break;
                    case DPAD_Direction.DOWN_RIGHT: controllerInputs.controllerButtons.DPAD_DOWN = true; controllerInputs.controllerButtons.DPAD_RIGHT = true; break;
                }
            }

            // Retrieve Emulated Keys
            controllerInputs = GetCurrentEmulatedKeys(controllerInputs);

            // Return to base.
            return controllerInputs;
        }

        /// <summary>
        /// Retrieves and returns the state of all of the controller's individual buttons.
        /// </summary>
        /// <param name="controllerInputs">The controller input struct.</param>
        private ControllerInputs GetCurrentButtons(ControllerInputs controllerInputs)
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
        private ControllerInputs GetCurrentAxis(ControllerInputs controllerInputs)
        {
            // Retrieve all of the axis.
            controllerInputs.SetLeftTriggerPressure(GetAxisState(ControllerAxis.Left_Trigger));
            controllerInputs.SetRightTriggerPressure(GetAxisState(ControllerAxis.Right_Trigger));

            controllerInputs.leftStick.SetX(GetAxisState(ControllerAxis.Left_Stick_X));
            controllerInputs.leftStick.SetY(GetAxisState(ControllerAxis.Left_Stick_Y));

            controllerInputs.rightStick.SetX(GetAxisState(ControllerAxis.Right_Stick_X));
            controllerInputs.rightStick.SetY(GetAxisState(ControllerAxis.Right_Stick_Y));

            // Return the axis.
            return controllerInputs;
        }

        /// <summary>
        /// Retrieves and returns the state of all of the inputs that are emulated on a digital (keyboard)
        /// to analog basis to allow the use of e.g. analog sticks within keyboards.
        /// If a non-zero value is set and the button is not pressed, the original value will not be overwritten.
        /// (i.e. Only overrides if input is sent)
        /// </summary>
        /// <returns></returns>
        private ControllerInputs GetCurrentEmulatedKeys(ControllerInputs controllerInputs)
        {
            // Retrieve Emulated DPAD Keys
            #region DPAD Keys
            if (EmulationMapping.DPAD_DOWN != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_DOWN, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_DOWN = true; }
            }
            if (EmulationMapping.DPAD_LEFT != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_LEFT, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_LEFT = true; }
            }
            if (EmulationMapping.DPAD_RIGHT != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_RIGHT, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) { controllerInputs.controllerButtons.DPAD_RIGHT = true; }
            }
            if (EmulationMapping.DPAD_UP != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.DPAD_UP, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

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
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetY(controllerInputs.leftStick.GetY() + ControllerCommon.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Left_Stick_Left != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Left, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetX() - ControllerCommon.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Left_Stick_Right != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Right, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetX(controllerInputs.leftStick.GetX() + ControllerCommon.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Left_Stick_Up != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Stick_Up, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.leftStick.SetY(controllerInputs.leftStick.GetY() - ControllerCommon.AXIS_MAX_VALUE_F); }
            }
            #endregion

            // Retrieve Emulated Right Analog Stick
            #region Right Analog Stick
            if (EmulationMapping.Right_Stick_Down != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Down, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.rightStick.SetY(controllerInputs.rightStick.GetY() + ControllerCommon.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Right_Stick_Left != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Left, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.rightStick.SetX(controllerInputs.rightStick.GetX() - ControllerCommon.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Right_Stick_Right != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Right, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.rightStick.SetX(controllerInputs.rightStick.GetX() + ControllerCommon.AXIS_MAX_VALUE_F); }
            }

            if (EmulationMapping.Right_Stick_Up != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Stick_Up, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.rightStick.SetY(controllerInputs.rightStick.GetY() - ControllerCommon.AXIS_MAX_VALUE_F); }
            }
            #endregion

            // Retrieve Emulated Triggers
            #region Triggers
            if (EmulationMapping.Right_Trigger != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Right_Trigger, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.SetRightTriggerPressure(ControllerCommon.AXIS_MAX_VALUE_F / DInputManager.TRIGGER_SCALE_FACTOR); }
            }

            if (EmulationMapping.Left_Trigger != BUTTON_NULL)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic.Left_Trigger, EmulationMapping);

                // Check if button is pressed.
                bool isPressed = JoystickState.Buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed)
                { controllerInputs.SetLeftTriggerPressure(ControllerCommon.AXIS_MAX_VALUE_F / DInputManager.TRIGGER_SCALE_FACTOR); }
            }
            #endregion

            // Return controller inputs
            return controllerInputs;
        }
    }
}
