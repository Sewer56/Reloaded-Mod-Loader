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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading;
using Reloaded.Input.Common;
using Reloaded.Input.Config;
using Reloaded.Input.DirectInput;
using Reloaded.Input.XInput;
using Reloaded.Paths;
using SharpDX.DirectInput;
using SharpDX.XInput;

namespace Reloaded.Input.Modules
{
    /// <summary>
    /// The Remapper class provides the means of loading and saving individual configurations for
    /// each DirectInput and XInput Device in question.
    /// Separate implementations are provided for both DirectInput and XInput.
    /// </summary>
    public class Remapper
    {
        /// <summary>
        /// Defines whether a configuration file should be loaded by the instance GUID or
        /// the product GUID for an individual controller.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum DirectInputConfigType
        {
            /// <summary>
            /// Differentiate by product (identical controllers will carry identical config).
            /// </summary>
            ProductGUID,

            /// <summary>
            /// Differentiate by instance (each controller is unique but also tied to USB port).
            /// </summary>
            InstanceGUID
        }

        /// <summary>
        /// Specifies the individual device type for the instantiation of this device.
        /// </summary>
        public enum InputDeviceType
        {
            DirectInput,
            XInput
        }

        /// <summary>
        /// The amount of percent an axis requires to be moved by to register by the remapper.
        /// </summary>
        private const float PercentageAxisDelta = 20.0F;

        /// <summary>
        /// The amount of time in milliseconds between when the controller is successively polled when assigning
        /// an axis or a button to a DirectInput or XInput controller.
        /// </summary>
        private const int SleepTimePolling = 16;

        /// <summary>
        /// Milliseconds in a second. It's not Rocket Science.
        /// </summary>
        private const int MillisecondsInSecond = 1000;

        /// <summary>
        /// This is the constructor for DirectInput devices. See class description for information
        /// about this class.
        /// </summary>
        /// <param name="deviceType">The type of the device (DirectInput)</param>
        /// <param name="dInputController">The directInput controller instance.</param>
        public Remapper(InputDeviceType deviceType, DInputController dInputController)
        {
            DeviceType = deviceType;
            Controller = dInputController;

            // Retrieve the configuration location.
            GetConfigLocation(dInputController);
        }

        /// <summary>
        /// The Remapper class provides the means of loading and saving individual configurations for
        /// each DirectInput and XInput Device in question.
        /// Separate implementations are provided for both DirectInput and XInput.
        /// </summary>
        /// <param name="deviceType">The type of the device (XInput)</param>
        /// <param name="xInputController">The directInput controller instance.</param>
        public Remapper(InputDeviceType deviceType, XInputController xInputController)
        {
            DeviceType = deviceType;
            Controller = xInputController;
            XInputPort = xInputController.ControllerId;

            // Retrieve the configuration location.
            GetConfigLocation(null);
        }

        /// <summary>
        /// Retrieves the file name of the controller, specifying
        /// the name of the file whereby the controller is stored.
        /// </summary>
        public string GetControllerName => Path.GetFileNameWithoutExtension(ConfigurationFileLocation);

        /// <summary>
        /// Specifies the type of the input device which is being remapped.
        /// </summary>
        public InputDeviceType DeviceType { get; }

        /// <summary>
        /// Specifies the individual XInput Port number. 
        /// Used for identifying the correct file to load for the XInput Device.
        /// </summary>
        private int XInputPort { get; }

        /// <summary>
        /// Specifies the current configuration type used for DirectInput devices.
        /// Controllers can be differentiated by product (identical controllers will carry identical config) or...
        /// Controllers can be differentiated by instance (each controller is unique but also tied to USB port).
        /// </summary>
        private DirectInputConfigType ConfigType { get; set; }

        /// <summary>
        /// Stores the individual location of the controller configuration file.
        /// This is determined with the use of the individual controller.
        /// </summary>
        private string ConfigurationFileLocation { get; set; }

        /// <summary>
        /// Stores the instance of a DirectInput controller used for
        /// remapping DirectInput devices specifically.
        /// </summary>
        private ControllerCommon.IController Controller { get; }

        /// <summary>
        /// Retrieves the configuration file location for the individual controller.
        /// If using an XInput controller, pass null.
        /// </summary>
        private void GetConfigLocation(DInputController dInputController)
        {
            // Get Configuration Details
            ControllerConfig controllerConfig = ControllerConfig.ParseConfig();

            // Set the device type
            ConfigType = controllerConfig.DirectInputConfigType;

            // If XInput/DInput
            if (DeviceType == InputDeviceType.DirectInput)
            {
                // If InstanceGUID or ProductGUID.
                if (ConfigType == DirectInputConfigType.InstanceGUID)
                    ConfigurationFileLocation = LoaderPaths.GetModLoaderConfigDirectory() + "/Controllers/Instances/" +
                                                dInputController.Information.InstanceGuid + ".json";

                else if (ConfigType == DirectInputConfigType.ProductGUID)
                    ConfigurationFileLocation = LoaderPaths.GetModLoaderConfigDirectory() + "/Controllers/" +
                                                PathSanitizer.ForceValidFilePath(dInputController.Information.ProductName) + ".json";
            }
            else if (DeviceType == InputDeviceType.XInput)
            {
                ConfigurationFileLocation = LoaderPaths.GetModLoaderConfigDirectory() + "/Controllers/XInput/" + "Controller_" + XInputPort + ".json";
            }
        }

        /// <summary>
        /// Writes the current controller mappings to the controller's ini file.
        /// </summary>
        public void SetMappings()
        {
            ControllerConfigParser.WriteConfig(ConfigurationFileLocation, Controller);
        }

        /// <summary>
        /// Retrieves the current controller mappings from the controller's ini file.
        /// </summary>
        public ControllerCommon.IController GetMappings()
        {
            return ControllerConfigParser.ParseConfig(ConfigurationFileLocation, Controller);
        }

        /// <summary>
        /// Waits for the user to move an axis and retrieves the last pressed axis. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new axis has been assigned to the current mapping entry.</returns>
        public bool RemapAxis(int timeoutSeconds, out float currentTimeout, ControllerCommon.AxisMappingEntry mappingEntry, ref bool cancellationToken)
        {
            // Retrieve the object type of the controller.
            Type controllerType = Controller.GetType();

            // If it's a DirectInput controller.
            if (controllerType == typeof(DInputController)) return DInputRemapAxis(timeoutSeconds, out currentTimeout, mappingEntry, ref cancellationToken);
            if (controllerType == typeof(XInputController)) return XInputRemapAxis(timeoutSeconds, out currentTimeout, mappingEntry, ref cancellationToken);

            currentTimeout = 0; return false;
        }

        /// <summary>
        /// Remaps the axis of a DirectInput controller.
        /// </summary>
        /// <returns>True if a new axis has been assigned to the current mapping entry.</returns>
        private bool DInputRemapAxis(int timeoutSeconds, out float currentTimeout, ControllerCommon.AxisMappingEntry mappingEntry, ref bool cancellationToken)
        {
            // Cast Controller to DInput Controller
            DInputController dInputController = (DInputController)Controller;
            
            // Get type of JoystickState
            Type stateType = typeof(JoystickState);

            // Retrieve Joystick State
            JoystickState joystickState = dInputController.GetCurrentState();

            // Initialize Timeout
            int pollAttempts = timeoutSeconds * MillisecondsInSecond / SleepTimePolling;
            int pollCounter = 0;
            
            // Get % Change for recognition of input.
            int percentDelta = (int)(DInputManager.AxisMaxValue / 100.0F * PercentageAxisDelta);

            // If the axis is relative, instead set the delta very low, as relative acceleration based inputs cannot be scaled to a range.
            if (dInputController.Properties.AxisMode == DeviceAxisMode.Relative)
            {
                // Set low delta
                percentDelta = 50;

                // Additionally reset every property.
                foreach (PropertyInfo propertyInfo in stateType.GetProperties())
                    if (propertyInfo.PropertyType == typeof(int)) propertyInfo.SetValue(joystickState, 0);
            }

            // Poll the controller properties.
            while (pollCounter < pollAttempts)
            {
                // Get new JoystickState
                JoystickState joystickStateNew = dInputController.GetCurrentState();

                // Iterate over all properties.
                foreach (PropertyInfo propertyInfo in stateType.GetProperties())
                {
                    // If the property type is an integer. (This covers, nearly all possible axis readings and all in common controllers.)
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        // Calculate the change of value from last time.
                        int valueDelta = (int) propertyInfo.GetValue(joystickState) -
                                         (int) propertyInfo.GetValue(joystickStateNew);

                        // If the value has changed over X amount
                        if (valueDelta < -1 * percentDelta)
                        {
                            //mappingEntry.isReversed = true;
                            mappingEntry.SourceAxis = propertyInfo.Name;
                            currentTimeout = 0;
                            return true;
                        }

                        if (valueDelta > percentDelta)
                        {
                            //mappingEntry.isReversed = false;
                            mappingEntry.SourceAxis = propertyInfo.Name;
                            currentTimeout = 0;
                            return true;
                        }
                    }
                }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = timeoutSeconds - pollCounter * SleepTimePolling / (float)MillisecondsInSecond;

                // Check exit condition
                if (cancellationToken) return false;

                // Sleep
                Thread.Sleep(SleepTimePolling);
            }

            // Set current timeout (suppress compiler)
            currentTimeout = 0;
            mappingEntry.SourceAxis = "Null";
            return false;
        }


        /// <summary>
        /// Remaps the axis of an XInput controller.
        /// </summary>
        private bool XInputRemapAxis(int timeoutSeconds, out float currentTimeout, ControllerCommon.AxisMappingEntry mappingEntry, ref bool cancellationToken)
        {
            // Cast Controller to DInput Controller
            XInputController xInputController = (XInputController)Controller;

            // Retrieve Joystick State
            State joystickState = xInputController.Controller.GetState();

            // Initialize Timeout
            // MillisecondsInSecond / SleepTimePolling = Amount of polls/ticks per second.
            int pollAttempts = timeoutSeconds * MillisecondsInSecond / SleepTimePolling;
            int pollCounter = 0;

            // Get % Change for recognition of input.
            int percentDelta = (int)(XInputController.MaxAnalogStickRangeXinput / 100.0F * PercentageAxisDelta);
            int percentDeltaTrigger = (int)(XInputController.MaxTriggerRangeXinput / 100.0F * PercentageAxisDelta);

            // Poll the controller properties.
            while (pollCounter < pollAttempts)
            {
                // Get new JoystickState
                State joystickStateNew = xInputController.Controller.GetState();

                // Get Deltas (Differences)
                int leftStickX = joystickState.Gamepad.LeftThumbX - joystickStateNew.Gamepad.LeftThumbX;
                int leftStickY = joystickState.Gamepad.LeftThumbY - joystickStateNew.Gamepad.LeftThumbY;
                int rightStickX = joystickState.Gamepad.RightThumbX - joystickStateNew.Gamepad.RightThumbX;
                int rightStickY = joystickState.Gamepad.RightTrigger - joystickStateNew.Gamepad.RightThumbY;
                int leftTrigger = joystickState.Gamepad.LeftTrigger - joystickStateNew.Gamepad.LeftTrigger;
                int rightTrigger = joystickState.Gamepad.RightTrigger - joystickStateNew.Gamepad.RightTrigger;

                // Iterate over all axis.
                if (leftStickX < -1 * percentDelta) { mappingEntry.IsReversed = true; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.LeftStickX; currentTimeout = 0; return true; }
                if (leftStickX > percentDelta) { mappingEntry.IsReversed = false; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.LeftStickX; currentTimeout = 0; return true; }

                if (rightStickX < -1 * percentDelta) { mappingEntry.IsReversed = true; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.RightStickX; currentTimeout = 0; return true; }
                if (rightStickX > percentDelta) { mappingEntry.IsReversed = false; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.RightStickX; currentTimeout = 0; return true; }

                if (leftStickY < -1 * percentDelta) { mappingEntry.IsReversed = true; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.LeftStickY; currentTimeout = 0; return true; }
                if (leftStickY > percentDelta) { mappingEntry.IsReversed = false; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.LeftStickY; currentTimeout = 0; return true; }

                if (rightStickY < -1 * percentDelta) { mappingEntry.IsReversed = true; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.RightStickY; currentTimeout = 0; return true; }
                if (rightStickY > percentDelta) { mappingEntry.IsReversed = false; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.RightStickY; currentTimeout = 0; return true; }

                if (leftTrigger < -1 * percentDeltaTrigger) { mappingEntry.IsReversed = true; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.LeftTrigger; currentTimeout = 0; return true; }
                if (leftTrigger > percentDeltaTrigger) { mappingEntry.IsReversed = false; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.LeftTrigger; currentTimeout = 0; return true; }

                if (rightTrigger < -1 * percentDeltaTrigger) { mappingEntry.IsReversed = true; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.RightTrigger; currentTimeout = 0; return true; }
                if (rightTrigger > percentDeltaTrigger) { mappingEntry.IsReversed = false; mappingEntry.DestinationAxis = ControllerCommon.ControllerAxis.RightTrigger; currentTimeout = 0; return true; }

                // Increase counter, calculate new time left.
                // SleepTimePolling / MillisecondsInSecond = Amount of time per 1 tick (pollCounter)
                // pollCOunter = current amount of ticks done.
                pollCounter += 1;
                currentTimeout = (float)timeoutSeconds - (pollCounter * (SleepTimePolling / MillisecondsInSecond));

                // Check exit condition
                if (cancellationToken) return false;

                // Sleep
                Thread.Sleep(SleepTimePolling);
            }

            // Set current timeout (suppress compiler)
            currentTimeout = 0;
            return false;
        }

        /// <summary>
        /// Waits for the user to press a button and retrieves the last pressed button. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new button has successfully been assigned by the user.</returns>
        public bool RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken)
        {
            // Retrieve the object type of the controller.
            Type controllerType = Controller.GetType();

            // If it's a DirectInput controller.
            if (controllerType == typeof(DInputController)) return DInputRemapButton(timeoutSeconds, out currentTimeout, ref buttonToMap, ref cancellationToken);
            if (controllerType == typeof(XInputController)) return XInputRemapButton(timeoutSeconds, out currentTimeout, ref buttonToMap, ref cancellationToken);

            currentTimeout = 0; return false;
        }


        /// <summary>
        /// Remaps a DirectInput button to the controller button map struct.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new button has successfully been assigned by the user.</returns>
        private bool DInputRemapButton(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken)
        {
            // Cast Controller to DInput Controller
            DInputController dInputController = (DInputController)Controller;

            // Retrieve Joystick State
            JoystickState joystickState = dInputController.GetCurrentState();

            // Initialize Timeout
            int pollAttempts = timeoutSeconds * MillisecondsInSecond / SleepTimePolling;
            int pollCounter = 0;

            // Poll the controller properties.
            while (pollCounter < pollAttempts)
            {
                // Get new JoystickState
                JoystickState joystickStateNew = dInputController.GetCurrentState();

                // Iterate over all buttons.
                for (int x = 0; x < joystickState.Buttons.Length; x++)
                {
                    if (joystickState.Buttons[x] != joystickStateNew.Buttons[x])
                    {
                        // Retrieve the button mapping.
                        ControllerCommon.ButtonMapping buttonMapping = Controller.InputMappings.ButtonMapping;

                        // Assign requested button.
                        buttonToMap = (byte) x;

                        // Reassign button mapping.
                        Controller.InputMappings.ButtonMapping = buttonMapping;

                        // Set timeout to 0
                        currentTimeout = 0;

                        // Return
                        return true;
                    }
                }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = timeoutSeconds - pollCounter * SleepTimePolling / (float)MillisecondsInSecond;

                // Check exit condition
                if (cancellationToken) return false;

                // Sleep
                Thread.Sleep(SleepTimePolling);
            }

            // Assign the current timeout.
            currentTimeout = 0;
            return false;
        }

        /// <summary>
        /// Remaps a XInput button mapped to a buton struct to the controller button map struct.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
        /// <returns>True if a new button has successfully been assigned by the user.</returns>
        private bool XInputRemapButton(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken)
        {
            // Cast Controller to DInput Controller
            XInputController xInputController = (XInputController)Controller;

            // Retrieve Joystick State
            bool[] buttonState = xInputController.GetButtons();

            // Initialize Timeout
            int pollAttempts = timeoutSeconds * MillisecondsInSecond / SleepTimePolling;
            int pollCounter = 0;

            // Poll the controller properties.
            while (pollCounter < pollAttempts)
            {
                // Get new JoystickState
                bool[] buttonStateNew = xInputController.GetButtons();

                // Iterate over all buttons.
                for (int x = 0; x < buttonStateNew.Length; x++)
                {
                    if (buttonState[x] != buttonStateNew[x])
                    {
                        // Retrieve the button mapping.
                        ControllerCommon.ButtonMapping buttonMapping = Controller.InputMappings.ButtonMapping;

                        // Assign requested button.
                        buttonToMap = (byte) x;

                        // Reassign button mapping.
                        Controller.InputMappings.ButtonMapping = buttonMapping;

                        // Set timeout to 0
                        currentTimeout = 0;

                        // Return
                        return true;
                    }
                }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = timeoutSeconds - pollCounter * SleepTimePolling / (float)MillisecondsInSecond;

                // Check exit condition
                if (cancellationToken) return false;

                // Sleep
                Thread.Sleep(SleepTimePolling);
            }

            // Assign the current timeout.
            currentTimeout = 0;
            return false;
        }
    }
}

