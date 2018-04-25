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
using static Reloaded.Input.Common.ControllerCommon;

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
        public static int DInputGetMappedButtonIndex(ControllerButtonsGeneric button, ButtonMapping buttonMappings)
        {
            // Stores the button that is to be checked.
            int buttonToTest = ButtonNull;

            // Switch statement checks the mapping of each button to the virtual xbox button.
            switch (button)
            {
                case ControllerButtonsGeneric.ButtonA: buttonToTest = buttonMappings.ButtonA; break;
                case ControllerButtonsGeneric.ButtonB: buttonToTest = buttonMappings.ButtonB; break;
                case ControllerButtonsGeneric.ButtonX: buttonToTest = buttonMappings.ButtonX; break;
                case ControllerButtonsGeneric.ButtonY: buttonToTest = buttonMappings.ButtonY; break;
                case ControllerButtonsGeneric.ButtonLb: buttonToTest = buttonMappings.ButtonLb; break;
                case ControllerButtonsGeneric.ButtonRb: buttonToTest = buttonMappings.ButtonRb; break;
                case ControllerButtonsGeneric.ButtonLs: buttonToTest = buttonMappings.ButtonLs; break;
                case ControllerButtonsGeneric.ButtonRs: buttonToTest = buttonMappings.ButtonRs; break;
                case ControllerButtonsGeneric.ButtonBack: buttonToTest = buttonMappings.ButtonBack; break;
                case ControllerButtonsGeneric.ButtonStart: buttonToTest = buttonMappings.ButtonStart; break;
                case ControllerButtonsGeneric.ButtonGuide: buttonToTest = buttonMappings.ButtonGuide; break;
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
        public static int DInputGetEmulatedButtonIndex(EmulatedButtonsGeneric button, EmulationButtonMapping buttonMappings)
        {
            // Stores the button that is to be checked.
            int buttonToTest = ButtonNull;

            // Switch statement checks the mapping of each button to the virtual xbox button.
            switch (button)
            {
                case EmulatedButtonsGeneric.DpadDown: buttonToTest = buttonMappings.DpadDown; break;
                case EmulatedButtonsGeneric.DpadLeft: buttonToTest = buttonMappings.DpadLeft; break;
                case EmulatedButtonsGeneric.DpadRight: buttonToTest = buttonMappings.DpadRight; break;
                case EmulatedButtonsGeneric.DpadUp: buttonToTest = buttonMappings.DpadUp; break;

                case EmulatedButtonsGeneric.LeftStickDown: buttonToTest = buttonMappings.LeftStickDown; break;
                case EmulatedButtonsGeneric.LeftStickLeft: buttonToTest = buttonMappings.LeftStickLeft; break;
                case EmulatedButtonsGeneric.LeftStickRight: buttonToTest = buttonMappings.LeftStickRight; break;
                case EmulatedButtonsGeneric.LeftStickUp: buttonToTest = buttonMappings.LeftStickUp; break;

                case EmulatedButtonsGeneric.RightStickDown: buttonToTest = buttonMappings.RightStickDown; break;
                case EmulatedButtonsGeneric.RightStickLeft: buttonToTest = buttonMappings.RightStickLeft; break;
                case EmulatedButtonsGeneric.RightStickRight: buttonToTest = buttonMappings.RightStickRight; break;
                case EmulatedButtonsGeneric.RightStickUp: buttonToTest = buttonMappings.RightStickUp; break;

                case EmulatedButtonsGeneric.LeftTrigger: buttonToTest = buttonMappings.LeftTrigger; break;
                case EmulatedButtonsGeneric.RightTrigger: buttonToTest = buttonMappings.RightTrigger; break;
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
            if (mappingEntry.SourceAxis == "Null" && (mappingEntry.DestinationAxis == ControllerAxis.LeftTrigger || mappingEntry.DestinationAxis == ControllerAxis.RightTrigger))
                return DInputManager.AxisMinValue;

            // Else return 0 if the axis source is null.
            if (mappingEntry.SourceAxis == "Null") return 0;

            // Return the appropriately mapped axis!
            return (int)Reflection_GetValue(joystickState, mappingEntry.SourceAxis); 
        }
    }
}
