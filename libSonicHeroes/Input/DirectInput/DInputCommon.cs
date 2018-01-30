using SharpDX.DirectInput;
using static SonicHeroes.Input.ControllerCommon;

namespace SonicHeroes.Input.DirectInput
{
    /// <summary>
    /// Defines the common methods used between all devices and classes which make use of DirectInput.
    /// </summary>
    public class DInputCommon
    {
        /// <summary>
        /// Returns the index of the button that is to be checked within the controller's mapping configuration.
        /// </summary>
        /// <param name="button">The requested button whose index is to be obtained.</param>
        /// <param name="buttonMappings">The button mapping for the specified controller.</param>
        /// <returns></returns>
        public static int DInputGetMappedButtonIndex(Controller_Buttons_Generic button, ButtonMapping buttonMappings)
        {
            // Stores the button that is to be checked.
            int buttonToTest = BUTTON_NULL;

            // Switch statement checks the mapping of each button to the virtual xbox button.
            switch (button)
            {
                case Controller_Buttons_Generic.Button_A: buttonToTest = buttonMappings.Button_A; break;
                case Controller_Buttons_Generic.Button_B: buttonToTest = buttonMappings.Button_B; break;
                case Controller_Buttons_Generic.Button_X: buttonToTest = buttonMappings.Button_X; break;
                case Controller_Buttons_Generic.Button_Y: buttonToTest = buttonMappings.Button_Y; break;
                case Controller_Buttons_Generic.Button_LB: buttonToTest = buttonMappings.Button_LB; break;
                case Controller_Buttons_Generic.Button_RB: buttonToTest = buttonMappings.Button_RB; break;
                case Controller_Buttons_Generic.Button_LS: buttonToTest = buttonMappings.Button_LS; break;
                case Controller_Buttons_Generic.Button_RS: buttonToTest = buttonMappings.Button_RS; break;
                case Controller_Buttons_Generic.Button_Back: buttonToTest = buttonMappings.Button_Back; break;
                case Controller_Buttons_Generic.Button_Start: buttonToTest = buttonMappings.Button_Start; break;
                case Controller_Buttons_Generic.Button_Guide: buttonToTest = buttonMappings.Button_Guide; break;
            }

            // Return the button that is to be checked.
            return buttonToTest;
        }

        /// <summary>
        /// Returns the index of the button that is to be checked within the controller's mapping configuration.
        /// </summary>
        /// <param name="button">The requested button whose index is to be obtained.</param>
        /// <param name="buttonMappings">The button mapping for the specified controller.</param>
        /// <returns></returns>
        public static int DInputGetEmulatedButtonIndex(Emulated_Buttons_Generic button, EmulationButtonMapping buttonMappings)
        {
            // Stores the button that is to be checked.
            int buttonToTest = BUTTON_NULL;

            // Switch statement checks the mapping of each button to the virtual xbox button.
            switch (button)
            {
                case Emulated_Buttons_Generic.DPAD_DOWN: buttonToTest = buttonMappings.DPAD_DOWN; break;
                case Emulated_Buttons_Generic.DPAD_LEFT: buttonToTest = buttonMappings.DPAD_LEFT; break;
                case Emulated_Buttons_Generic.DPAD_RIGHT: buttonToTest = buttonMappings.DPAD_RIGHT; break;
                case Emulated_Buttons_Generic.DPAD_UP: buttonToTest = buttonMappings.DPAD_UP; break;

                case Emulated_Buttons_Generic.Left_Stick_Down: buttonToTest = buttonMappings.Left_Stick_Down; break;
                case Emulated_Buttons_Generic.Left_Stick_Left: buttonToTest = buttonMappings.Left_Stick_Left; break;
                case Emulated_Buttons_Generic.Left_Stick_Right: buttonToTest = buttonMappings.Left_Stick_Right; break;
                case Emulated_Buttons_Generic.Left_Stick_Up: buttonToTest = buttonMappings.Left_Stick_Up; break;

                case Emulated_Buttons_Generic.Right_Stick_Down: buttonToTest = buttonMappings.Right_Stick_Down; break;
                case Emulated_Buttons_Generic.Right_Stick_Left: buttonToTest = buttonMappings.Right_Stick_Left; break;
                case Emulated_Buttons_Generic.Right_Stick_Right: buttonToTest = buttonMappings.Right_Stick_Right; break;
                case Emulated_Buttons_Generic.Right_Stick_Up: buttonToTest = buttonMappings.Right_Stick_Up; break;

                case Emulated_Buttons_Generic.Left_Trigger: buttonToTest = buttonMappings.Left_Trigger; break;
                case Emulated_Buttons_Generic.Right_Trigger: buttonToTest = buttonMappings.Right_Trigger; break;
            }

            // Return the button that is to be checked.
            return buttonToTest;
        }

        /// <summary>
        /// Returns the appropriate axis mapping entry for each individual specific axis.
        /// The axis have a 1-1 relationship (oldaxis -> new axis) and a set of properties as defined in
        /// Controller_Axis_Mapping.
        /// </summary>
        /// <param name="axis">The axis whose details we want to obtain.</param>
        /// <param name="axisMapping">The axis mapping which stores the axis details for the individual controller.</param>
        /// <returns></returns>
        public static AxisMappingEntry DInputGetMappedAxis(ControllerAxis axis, AxisMapping axisMapping)
        {
            // Stores the axis configuration that is to be returned.
            AxisMappingEntry controllerAxisMapping = new AxisMappingEntry(); 

            // Switch statement checks the mapping of each axis to verify where it points to. Then retrieves that axis!
            switch (axis)
            {
                case ControllerAxis.Left_Stick_X: controllerAxisMapping = axisMapping.leftStickX; break;
                case ControllerAxis.Left_Stick_Y: controllerAxisMapping = axisMapping.leftStickY; break;

                case ControllerAxis.Right_Stick_X: controllerAxisMapping = axisMapping.rightStickX; break;
                case ControllerAxis.Right_Stick_Y: controllerAxisMapping = axisMapping.rightStickY; break;

                case ControllerAxis.Left_Trigger_Pressure: controllerAxisMapping = axisMapping.leftTrigger; break;
                case ControllerAxis.Right_Trigger_Pressure: controllerAxisMapping = axisMapping.rightTrigger; break;
            }

            // Retrieve empty struct if null, else the correct axis mapping.
            return controllerAxisMapping; 
        }

        /// <summary>
        /// Returns the current value for a given axis based off of the set axis in the axis mapping entry and the joystick state.
        /// Performs any necessary additional operations on the axis values based off of the axis configuration.
        /// </summary>
        /// <param name="mappingEntry">The mapping entry for the axis defining which axis should be used.</param>
        /// <param name="joystickState">The current state of the controller in question.</param>
        public static float DInputGetAxisValue(AxisMappingEntry mappingEntry, JoystickState joystickState)
        {
            // Obtain the raw value for the requested axis.
            int rawValue = GetAxisRawValue(mappingEntry, joystickState);

            // Process the raw axis value and return.
            return DInputProcessAxisRawValue(rawValue, mappingEntry);
        }

        /// <summary>
        /// Processes the obtained raw value with DInput ranges and performs modifications on it such
        /// as changing the radius of the axis reading or checking for deadzones.
        /// </summary>
        /// <param name="mappingEntry">The mapping entry for the axis defining which axis should be used.</param>
        /// <param name="rawValue">The raw value obtained from the axis query..</param>
        public static float DInputProcessAxisRawValue(int rawValue, AxisMappingEntry mappingEntry)
        {
            // Reverse Axis if Necessary
            if (mappingEntry.isReversed) { rawValue = -1 * rawValue; }

            // Scale Axis
            float newRawValue = ((float)rawValue / (float)DInputManager.AXIS_MAX_VALUE) * DInputManager.AXIS_MAX_VALUE_F;

            // If triggers. scale to between 0 - 100
            switch (mappingEntry.axis)
            {
                case ControllerAxis.Left_Trigger_Pressure:
                case ControllerAxis.Right_Trigger_Pressure:
                    newRawValue += 100;
                    newRawValue *= DInputManager.TRIGGER_SCALE_FACTOR;
                    break;
            }

            // If the input lays within the acceptable deadzone range, return null.
            if (VerifyDeadzones(newRawValue, mappingEntry)) { return 0F; }

            // Scale Radius Scale Value
            newRawValue *= mappingEntry.radiusScale;

            // Return axis value.
            return newRawValue;
        }

        /// <summary>
        /// Retrieves the raw value for a specified passed in requested axis in integer form.
        /// </summary>
        /// <param name="mappingEntry">The specific axis mapping entry that is to be used.</param>
        /// <param name="joystickState">The current state of the controller in question.</param>
        private static int GetAxisRawValue(AxisMappingEntry mappingEntry, JoystickState joystickState)
        {
            // Value for the current axis.
            int rawValue = 0;

            // Return the appropriately mapped axis!
            switch (mappingEntry.axis)
            {
                case ControllerAxis.Left_Stick_X: 
                case ControllerAxis.Left_Stick_Y: 
                case ControllerAxis.Right_Stick_X: 
                case ControllerAxis.Right_Stick_Y: 
                case ControllerAxis.Left_Trigger_Pressure: 
                case ControllerAxis.Right_Trigger_Pressure:
                    rawValue = (int)Reflection_GetValue(joystickState, mappingEntry.propertyName);
                    break;
                default: break;
            }

            // Returm the raw value.
            return rawValue;
        }

        /// <summary>
        /// Returns true the input raw value is within a deadzone specified
        /// by the specific axis mapping entry. 
        /// </summary>
        /// <param name="axisValue">The scaled value of the prior obtained raw axis reading: Range -100 to 100 (Sticks) or 0-100 (Triggers)</param>
        /// <param name="mappingEntry">The mapping entry for the axis defining the deadzone for the specific axis.</param>
        public static bool VerifyDeadzones(float axisValue, AxisMappingEntry mappingEntry)
        {
            // Verify Deadzones
            switch (mappingEntry.axis)
            {
                // For all analog sticks.
                case ControllerAxis.Left_Stick_X:
                case ControllerAxis.Left_Stick_Y:
                case ControllerAxis.Right_Stick_X:
                case ControllerAxis.Right_Stick_Y:

                    // Get boundaries of deadzone.
                    float deadzoneMax = (DInputManager.AXIS_MAX_VALUE_F / 100.0F) * mappingEntry.deadZone;
                    float deadzoneMin = (DInputManager.AXIS_MIN_VALUE_F / 100.0F) * mappingEntry.deadZone;

                    // If within boundaries, axis value is 0.
                    if ((axisValue < deadzoneMax) && (axisValue > deadzoneMin)) { return true; }

                    // Else break.
                    break;

                // For all triggers
                case ControllerAxis.Left_Trigger_Pressure:
                case ControllerAxis.Right_Trigger_Pressure:

                    // Get max deadzone
                    float deadzoneTriggerMax = ((DInputManager.AXIS_MAX_VALUE_F * 2) / 100.0F) * mappingEntry.deadZone;

                    // If within bounds, axis value is 0.
                    if (axisValue < deadzoneTriggerMax) { return true; }

                    // Else break.
                    break;
            }

            // If not in deadzones return false.
            return false;
        }
    }
}
