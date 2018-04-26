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

using System;
using Reloaded.Input.DirectInput;
using Reloaded.Input.Modules;
using SharpDX.XInput;
using static Reloaded.Input.Common.ControllerCommon;
using static Reloaded.Input.DirectInput.DInputCommon;

namespace Reloaded.Input.XInput
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
        public const float MaxAnalogStickRangeXinput = 32767;

        /// <summary>
        /// Defines the maximum value returnable by an XInput trigger.
        /// </summary>
        public const float MaxTriggerRangeXinput = 255;

        /// <summary>
        /// Constructor for this class, defines the individual controller.
        /// </summary>
        /// <param name="controllerId">Specifies the XInput controller port for the controller to be created.</param>
        public XInputController(int controllerId)
        {
            // Create controller instance and assign controller port.
            Controller = new Controller((UserIndex)controllerId);
            ControllerId = controllerId;

            // Instantiate the remapper.
            Remapper = new Remapper(Remapper.InputDeviceType.XInput, this);

            // Get the controller key binding.
            Remapper.GetMappings();
        }

        /// <summary> 
        /// Holds the current instance of the XImput Controller. 
        /// Store the individual input mappings used to map buttons to buttons, 
        /// buttons to axis and axis to axis etc. 
        /// </summary> 
        public Controller Controller { get; set; }

        /// <summary> 
        /// Holds the current instance of the XImput Controller. 
        /// </summary> 
        private State ControllerState { get; set; }

        /// <summary>
        /// Store the individual input mappings used to map buttons to buttons,
        /// buttons to axis and axis to axis etc.
        /// </summary>
        public InputMappings InputMappings { get; set; } = new InputMappings()
        {
            ButtonMapping = new ButtonMapping(),
            AxisMapping = new AxisMapping(),
            ControllerId = 0,
            EmulationMapping = new EmulationButtonMapping()
        };

        /// <summary>
        /// Defines the individual port used for this specific controller.
        /// </summary>
        public int ControllerId { get; set; }

        /// <summary>
        /// Provides a control scheme remapper allowing for buttons to be remapped on the fly.
        /// </summary>
        public Remapper Remapper { get; set; }

        /// <summary>
        /// Returns true if the XInput Controller is connected.
        /// </summary>
        public bool IsConnected() { return Controller.IsConnected; }

        /// <summary>
        /// Waits for the user to move an axis and retrieves the last pressed axis. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new axis has successfully been assigned by the user.</returns>
        public bool RemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry, ref bool cancellationToken) { return Remapper.RemapAxis(timeoutSeconds, out currentTimeout, mappingEntry, ref cancellationToken); }

        /// <summary>
        /// Waits for the user to press a button and retrieves the last pressed button. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new button has successfully been assigned by the user.</returns>
        public bool RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken) { return Remapper.RemapButtons(timeoutSeconds, out currentTimeout, ref buttonToMap, ref cancellationToken); }

        /// <summary>
        /// Retrieves whether a specific button is pressed or not. 
        /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
        /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
        /// True if said button is pressed, else false.
        /// </summary>
        public bool GetButtonState(ControllerButtonsGeneric button)
        {
            // Retrieve current button state.
            bool[] buttons = GetButtons();

            // Retrieve requested button index.
            int buttonIndex = DInputGetMappedButtonIndex(button, InputMappings.ButtonMapping);

            // Return the state declaring if the joystick button is pressed.
            return buttonIndex != 255 && buttons[buttonIndex];
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
            AxisMappingEntry controllerAxisMapping = InputGetMappedAxis(axis, InputMappings.AxisMapping);

            // Retrieve the intensity of the axis press-in value.
            return XInputGetAxisValue(controllerAxisMapping);
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
            bool[] buttons = new bool[Enum.GetNames(typeof(ControllerButtonsGeneric)).Length];

            // Get XBOX Buttons
            GamepadButtonFlags buttonFlags = ControllerState.Gamepad.Buttons;

            // Retrieve Controller Button Status.
            buttons[(int)ControllerButtonsGeneric.ButtonA] = buttonFlags.HasFlag(GamepadButtonFlags.A);
            buttons[(int)ControllerButtonsGeneric.ButtonB] = buttonFlags.HasFlag(GamepadButtonFlags.B);
            buttons[(int)ControllerButtonsGeneric.ButtonX] = buttonFlags.HasFlag(GamepadButtonFlags.X);
            buttons[(int)ControllerButtonsGeneric.ButtonY] = buttonFlags.HasFlag(GamepadButtonFlags.Y);

            buttons[(int)ControllerButtonsGeneric.ButtonLs] = buttonFlags.HasFlag(GamepadButtonFlags.LeftThumb);
            buttons[(int)ControllerButtonsGeneric.ButtonRs] = buttonFlags.HasFlag(GamepadButtonFlags.RightThumb);

            buttons[(int)ControllerButtonsGeneric.ButtonLb] = buttonFlags.HasFlag(GamepadButtonFlags.LeftShoulder);
            buttons[(int)ControllerButtonsGeneric.ButtonRb] = buttonFlags.HasFlag(GamepadButtonFlags.RightShoulder);

            buttons[(int)ControllerButtonsGeneric.ButtonBack] = buttonFlags.HasFlag(GamepadButtonFlags.Back);
            buttons[(int)ControllerButtonsGeneric.ButtonStart] = buttonFlags.HasFlag(GamepadButtonFlags.Start);
            buttons[(int)ControllerButtonsGeneric.ButtonGuide] = false;

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
            // Instantiate an instance of controller inputs.
            ControllerInputs controllerInputs = new ControllerInputs();

            // Check if controller is connected.
            if (Controller.IsConnected)
            {
                // Update the current state of the Joystick/Controller
                ControllerState = Controller.GetState();

                // Retrieve all of the buttons;
                controllerInputs = GetControllerStateButtons(controllerInputs);

                // Retrieve all of the axis.
                controllerInputs = GetControllerStateAxis(controllerInputs);

                // Retrieve DPAD Information
                GamepadButtonFlags buttonFlags = ControllerState.Gamepad.Buttons;

                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadUp)) controllerInputs.ControllerButtons.DpadUp = true;
                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadLeft)) controllerInputs.ControllerButtons.DpadLeft = true;
                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadRight)) controllerInputs.ControllerButtons.DpadRight = true;
                if (buttonFlags.HasFlag(GamepadButtonFlags.DPadDown)) controllerInputs.ControllerButtons.DpadDown = true;

                // Retrieve Emulated Keys
                controllerInputs = GetControllerState_EmulatedKeys(controllerInputs, GetButtons());
            }

            // Return to base.
            return controllerInputs;
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
            return InputProcessAxisRawValue(rawValue, mappingEntry, true);
        }

        /// <summary>
        /// Retrieves and returns the state of all of the controller's individual buttons.
        /// </summary>
        /// <param name="controllerInputs">The controller input struct.</param>
        private ControllerInputs GetControllerStateButtons(ControllerInputs controllerInputs)
        {
            // Retrieve all of the buttons.
            controllerInputs.ControllerButtons.ButtonA = GetButtonState(ControllerButtonsGeneric.ButtonA);
            controllerInputs.ControllerButtons.ButtonB = GetButtonState(ControllerButtonsGeneric.ButtonB);
            controllerInputs.ControllerButtons.ButtonX = GetButtonState(ControllerButtonsGeneric.ButtonX);
            controllerInputs.ControllerButtons.ButtonY = GetButtonState(ControllerButtonsGeneric.ButtonY);

            controllerInputs.ControllerButtons.ButtonLb = GetButtonState(ControllerButtonsGeneric.ButtonLb);
            controllerInputs.ControllerButtons.ButtonRb = GetButtonState(ControllerButtonsGeneric.ButtonRb);

            controllerInputs.ControllerButtons.ButtonBack = GetButtonState(ControllerButtonsGeneric.ButtonBack);
            controllerInputs.ControllerButtons.ButtonGuide = GetButtonState(ControllerButtonsGeneric.ButtonGuide);
            controllerInputs.ControllerButtons.ButtonStart = GetButtonState(ControllerButtonsGeneric.ButtonStart);

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
            controllerInputs.SetLeftTriggerPressure(GetAxisState(ControllerAxis.LeftTrigger));
            controllerInputs.SetRightTriggerPressure(GetAxisState(ControllerAxis.RightTrigger));

            controllerInputs.LeftStick.SetX(GetAxisState(ControllerAxis.LeftStickX));
            controllerInputs.LeftStick.SetY(GetAxisState(ControllerAxis.LeftStickY));

            controllerInputs.RightStick.SetX(GetAxisState(ControllerAxis.RightStickX));
            controllerInputs.RightStick.SetY(GetAxisState(ControllerAxis.RightStickY));

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

            // Check what the mapping entry is natively within the axis mapping, and obtain the relevant raw inputs.
            if (mappingEntry == InputMappings.AxisMapping.LeftStickX) { rawValue = ControllerState.Gamepad.LeftThumbX; }
            else if (mappingEntry == InputMappings.AxisMapping.LeftStickY) { rawValue = -ControllerState.Gamepad.LeftThumbY; }
            else if (mappingEntry == InputMappings.AxisMapping.RightStickX) { rawValue = ControllerState.Gamepad.RightThumbX; }
            else if (mappingEntry == InputMappings.AxisMapping.RightStickY) { rawValue = -ControllerState.Gamepad.RightThumbY; }
            else if (mappingEntry == InputMappings.AxisMapping.LeftTrigger) { rawValue = ControllerState.Gamepad.LeftTrigger; }
            else if (mappingEntry == InputMappings.AxisMapping.RightTrigger) { rawValue = ControllerState.Gamepad.RightTrigger; }

            // Process the value to DInput Ranges
            if (!(mappingEntry == InputMappings.AxisMapping.LeftTrigger || mappingEntry == InputMappings.AxisMapping.RightTrigger))
            {
                // Axis is analog stick.
                // Scale from -32768-32767 to -100-100
                rawValue = (int)(rawValue / MaxAnalogStickRangeXinput * AxisMaxValueF);
            }
            else
            {
                // Axis is trigger.
                // Scale from 0-255 to 0-200
                rawValue = (int)(rawValue / MaxTriggerRangeXinput * AxisMaxValueF * (1.0F / DInputManager.TriggerScaleFactor));

                // Scale to -100-100 to simulate DInput.
                rawValue -= 100;
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
            if (InputMappings.EmulationMapping.DpadDown != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.DpadDown, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) controllerInputs.ControllerButtons.DpadDown = true;
            }
            if (InputMappings.EmulationMapping.DpadLeft != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.DpadLeft, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) controllerInputs.ControllerButtons.DpadLeft = true;
            }
            if (InputMappings.EmulationMapping.DpadRight != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.DpadRight, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) controllerInputs.ControllerButtons.DpadRight = true;
            }
            if (InputMappings.EmulationMapping.DpadUp != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.DpadUp, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If it is pressed, override the current value to include the flag.
                if (isPressed) controllerInputs.ControllerButtons.DpadUp = true;
            }
            #endregion

            // Retrieve Emulated Left Analog Stick
            #region Left Analog Stick
            if (InputMappings.EmulationMapping.LeftStickDown != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.LeftStickDown, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.LeftStick.SetY(controllerInputs.LeftStick.GetY() + AxisMaxValueF);
            }

            if (InputMappings.EmulationMapping.LeftStickLeft != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.LeftStickLeft, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.LeftStick.SetX(controllerInputs.LeftStick.GetX() - AxisMaxValueF);
            }

            if (InputMappings.EmulationMapping.LeftStickRight != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.LeftStickRight, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.LeftStick.SetX(controllerInputs.LeftStick.GetX() + AxisMaxValueF);
            }

            if (InputMappings.EmulationMapping.LeftStickUp != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.LeftStickUp, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.LeftStick.SetY(controllerInputs.LeftStick.GetY() - AxisMaxValueF);
            }
            #endregion

            // Retrieve Emulated Right Analog Stick
            #region Right Analog Stick
            if (InputMappings.EmulationMapping.RightStickDown != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.RightStickDown, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.RightStick.SetY(controllerInputs.RightStick.GetY() + AxisMaxValueF);
            }

            if (InputMappings.EmulationMapping.RightStickLeft != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.RightStickLeft, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.RightStick.SetX(controllerInputs.RightStick.GetX() - AxisMaxValueF);
            }

            if (InputMappings.EmulationMapping.RightStickRight != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.RightStickRight, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.RightStick.SetX(controllerInputs.RightStick.GetX() + AxisMaxValueF);
            }

            if (InputMappings.EmulationMapping.RightStickUp != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.RightStickUp, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.RightStick.SetY(controllerInputs.RightStick.GetY() - AxisMaxValueF);
            }
            #endregion

            // Retrieve Emulated Triggers
            #region Triggers
            if (InputMappings.EmulationMapping.RightTrigger != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.RightTrigger, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.SetRightTriggerPressure(AxisMaxValueF / DInputManager.TriggerScaleFactor);
            }

            if (InputMappings.EmulationMapping.LeftTrigger != ButtonNull)
            {
                // Retrieve the button index for the emulated button.
                int buttonIndex = DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric.LeftTrigger, InputMappings.EmulationMapping);

                // Check if button is pressed.
                bool isPressed = buttons[buttonIndex];

                // If the stick value is not 0 and is not pressed, do not override.
                if (isPressed) controllerInputs.SetLeftTriggerPressure(AxisMaxValueF / DInputManager.TriggerScaleFactor);
            }
            #endregion

            // Return controller inputs
            return controllerInputs;
        }
    }
}
