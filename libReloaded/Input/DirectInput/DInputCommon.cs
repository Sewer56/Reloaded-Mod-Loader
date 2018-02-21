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
using static Reloaded.Input.ControllerCommon;

namespace Reloaded.Input.DirectInput
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
            return InputProcessAxisRawValue(rawValue, mappingEntry, false);
        }

        /// <summary>
        /// Retrieves the raw value for a specified passed in requested axis in integer form.
        /// </summary>
        /// <param name="mappingEntry">The specific axis mapping entry that is to be used.</param>
        /// <param name="joystickState">The current state of the controller in question.</param>
        private static int GetAxisRawValue(AxisMappingEntry mappingEntry, JoystickState joystickState)
        {
            // If axis source is null, and the axis is a trigger, return minimum float.
            if (mappingEntry.propertyName == "Null" && (mappingEntry.axis == ControllerAxis.Left_Trigger || mappingEntry.axis == ControllerAxis.Right_Trigger)) { return DInputManager.AXIS_MIN_VALUE; }

            // Else return 0 if the axis source is null.
            else if (mappingEntry.propertyName == "Null") { return 0; }

            // Return the appropriately mapped axis!
            return (int)Reflection_GetValue(joystickState, mappingEntry.propertyName); 
        }
    }
}
