using SharpDX.DirectInput;
using Reloaded.Input.DirectInput;
using Reloaded.Input.XInput;
using Reloaded.Misc;
using System;
using System.Reflection;
using System.Threading;
using static Reloaded.Input.ControllerCommon;

namespace Reloaded.Input
{
    /// <summary>
    /// The Remapper class provides the means of loading and saving individual configurations for
    /// each DirectInput and XInput Device in question.
    /// Separate implementations are provided for both DirectInput and XInput.
    /// </summary>
    public class Remapper
    {
        /// <summary>
        /// The amount of percent an axis requires to be moved by to register by the remapper.
        /// </summary>
        private const float PERCENTAGE_AXIS_DELTA = 20.0F;

        /// <summary>
        /// The amount of time in milliseconds between when the controller is successively polled when assigning
        /// an axis or a button to a DirectInput or XInput controller.
        /// </summary>
        private const int SLEEP_TIME_POLLING = 16;

        /// <summary>
        /// Milliseconds in a second. It's not Rocket Science.
        /// </summary>
        private const int MILLISECONDS_IN_SECOND = 1000;

        /// <summary>
        /// Instance of the controller configurator parser allowing for controller configurations
        /// to be saved and/or loaded.
        /// </summary>
        private ControllerConfigParser ConfigParser { get; set; }

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
        /// Specifies the type of the input device which is being remapped.
        /// </summary>
        private InputDeviceType DeviceType { get; }

        /// <summary>
        /// Stores the individual location of the controller configuration file.
        /// This is determined with the use of the individual controller 
        /// </summary>
        private string ConfigurationFileLocation { get; set; }

        /// <summary>
        /// Stores the instance of a DirectInput controller used for
        /// remapping DirectInput devices specifically.
        /// </summary>
        private IController Controller { get; set; }

        /// <summary>
        /// Specifies the individual device type for the instantiation of this device.
        /// </summary>
        public enum InputDeviceType
        {
            DirectInput,
            XInput
        }

        /// <summary>
        /// Defines whether a configuration file should be loaded by the instance GUID or
        /// the product GUID for an individual controller.
        /// </summary>
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
        /// This is the constructor for DirectInput devices. See class description for information
        /// about this class.
        /// </summary>
        /// <param name="deviceType">The type of the device (DirectInput)</param>
        /// <param name="dInputController">The directInput controller instance.</param>
        public Remapper(InputDeviceType deviceType, DInputController dInputController)
        {
            this.DeviceType = deviceType;
            this.Controller = dInputController;

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
            this.DeviceType = deviceType;
            this.Controller = xInputController;
            this.XInputPort = xInputController.ControllerID;

            // Retrieve the configuration location.
            GetConfigLocation(null);
        }

        /// <summary>
        /// Retrieves the configuration file location for the individual controller.
        /// If using an XInput controller, pass null.
        /// </summary>
        private void GetConfigLocation(DInputController dInputController)
        {
            // Get Configuration Details
            Misc.Config.LoaderConfigParser loaderConfigParser = new Misc.Config.LoaderConfigParser();
            Misc.Config.LoaderConfigParser.Config config = loaderConfigParser.ParseConfig();

            // Set the device type
            ConfigType = config.DirectInputConfigType;

            // If XInput/DInput
            if (DeviceType == InputDeviceType.DirectInput)
            {
                // If InstanceGUID or ProductGUID.
                if (ConfigType == DirectInputConfigType.InstanceGUID)
                {
                    ConfigurationFileLocation = LoaderPaths.GetModLoaderConfigDirectory() + "/Controllers/Instances/" + dInputController.Information.InstanceGuid + ".ini";
                }
                else if (ConfigType == DirectInputConfigType.ProductGUID)
                {
                    ConfigurationFileLocation = LoaderPaths.GetModLoaderConfigDirectory() + "/Controllers/" + dInputController.Information.ProductName + ".ini";
                }
            }
            else if (DeviceType == InputDeviceType.XInput)
            {
                ConfigurationFileLocation = LoaderPaths.GetModLoaderConfigDirectory() + "/Controllers/XInput/" + "Controller_" + XInputPort + ".ini";
            }
        }

        /// <summary>
        /// Writes the current controller mappings to the controller's ini file.
        /// </summary>
        public void SetMappings()
        {
            ConfigParser.WriteConfig(ConfigurationFileLocation, Controller);
        }

        /// <summary>
        /// Retrieves the current controller mappings from the controller's ini file.
        /// </summary>
        public IController GetMappings()
        {
            return ConfigParser.ParseConfig(ConfigurationFileLocation, Controller);
        }

        /// <summary>
        /// Waits for the user to move an axis and retrieves the last pressed axis. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
        public void RemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry)
        {
            // Retrieve the object type of the controller.
            Type controllerType = Controller.GetType();

            // If it's a DirectInput controller.
            if (controllerType == typeof(DInputController)) { DInputRemapAxis(timeoutSeconds, out currentTimeout, mappingEntry); }
            else if (controllerType == typeof(XInputController)) { XInputRemapAxis(timeoutSeconds, out currentTimeout, mappingEntry); }
            else { currentTimeout = 0; } // Not DInput or XInput
        }

        /// <summary>
        /// Remaps the axis of a DirectInput controller.
        /// </summary>
        private void DInputRemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry)
        {
            // Cast Controller to DInput Controller
            DInputController dInputController = (DInputController)Controller;
            
            // Get type of JoystickState
            Type stateType = typeof(JoystickState);

            // Retrieve Joystick State
            JoystickState joystickState = dInputController.GetCurrentState();

            // Initialize Timeout
            int pollAttempts = (timeoutSeconds * MILLISECONDS_IN_SECOND) / SLEEP_TIME_POLLING;
            int pollCounter = 0;
            
            // Get % Change for recognition of input.
            int percentDelta = (int)((DInputManager.AXIS_MAX_VALUE / 100.0F) * PERCENTAGE_AXIS_DELTA);

            // Poll the controller properties.
            while (pollCounter < pollAttempts)
            {
                // Get new JoystickState
                JoystickState joystickStateNew = dInputController.GetCurrentState();

                // Iterate over all properties.
                foreach (PropertyInfo propertyInfo in stateType.GetProperties())
                {
                    // If the property type is an integer. (This covers, nearly all possible axis readings and all in common controllers.)
                    if (propertyInfo.GetType() == typeof(int))
                    {
                        // Calculate the change of value from last time.
                        int valueDelta = (int)propertyInfo.GetValue(joystickState) - (int)propertyInfo.GetValue(joystickStateNew);

                        // If the value has changed over X amount
                        if (valueDelta < (-1 * percentDelta) )
                        {
                            mappingEntry.isReversed = true;
                            mappingEntry.propertyName = propertyInfo.Name;
                            currentTimeout = 0;
                            return;
                        }
                        else if (valueDelta > percentDelta)
                        {
                            mappingEntry.isReversed = false;
                            mappingEntry.propertyName = propertyInfo.Name;
                            currentTimeout = 0;
                            return;
                        }
                    }
                }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = (float)timeoutSeconds - ( (pollCounter * SLEEP_TIME_POLLING) / MILLISECONDS_IN_SECOND);

                // Sleep
                Thread.Sleep(SLEEP_TIME_POLLING);
            }

            // Set current timeout (suppress compiler)
            currentTimeout = 0;
        }


        /// <summary>
        /// Remaps the axis of an XInput controller.
        /// </summary>
        private void XInputRemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry)
        {
            // Cast Controller to DInput Controller
            XInputController xInputController = (XInputController)Controller;

            // Retrieve Joystick State
            SharpDX.XInput.State joystickState = xInputController.Controller.GetState();

            // Initialize Timeout
            int pollAttempts = (timeoutSeconds * MILLISECONDS_IN_SECOND) / SLEEP_TIME_POLLING;
            int pollCounter = 0;

            // Get % Change for recognition of input.
            int percentDelta = (int)((XInputController.MAX_ANALOG_STICK_RANGE_XINPUT / 100.0F) * PERCENTAGE_AXIS_DELTA);
            int percentDeltaTrigger = (int)((XInputController.MAX_TRIGGER_RANGE_XINPUT / 100.0F) * PERCENTAGE_AXIS_DELTA);

            // Poll the controller properties.
            while (pollCounter < pollAttempts)
            {
                // Get new JoystickState
                SharpDX.XInput.State joystickStateNew = xInputController.Controller.GetState();

                // Get Deltas (Differences)
                int leftStickX = joystickState.Gamepad.LeftThumbX - joystickStateNew.Gamepad.LeftThumbX;
                int leftStickY = joystickState.Gamepad.LeftThumbY - joystickStateNew.Gamepad.LeftThumbY;
                int rightStickX = joystickState.Gamepad.RightThumbX - joystickStateNew.Gamepad.RightThumbX;
                int rightStickY = joystickState.Gamepad.RightTrigger - joystickStateNew.Gamepad.RightThumbY;
                int leftTrigger = joystickState.Gamepad.LeftTrigger - joystickStateNew.Gamepad.LeftTrigger;
                int rightTrigger = joystickState.Gamepad.RightTrigger - joystickStateNew.Gamepad.RightTrigger;

                // Iterate over all axis.
                if (leftStickX < (-1 * percentDelta)) { mappingEntry.isReversed = true; mappingEntry.axis = ControllerAxis.Left_Stick_X; currentTimeout = 0; return; }
                else if (leftStickX > percentDelta) { mappingEntry.isReversed = false; mappingEntry.axis = ControllerAxis.Left_Stick_X; currentTimeout = 0; return; }

                if (rightStickX < (-1 * percentDelta)) { mappingEntry.isReversed = true; mappingEntry.axis = ControllerAxis.Right_Stick_X; currentTimeout = 0; return; }
                else if (rightStickX > percentDelta) { mappingEntry.isReversed = false; mappingEntry.axis = ControllerAxis.Right_Stick_X; currentTimeout = 0; return; }

                if (leftStickY < (-1 * percentDelta)) { mappingEntry.isReversed = true; mappingEntry.axis = ControllerAxis.Left_Stick_Y; currentTimeout = 0; return; }
                else if (leftStickY > percentDelta) { mappingEntry.isReversed = false; mappingEntry.axis = ControllerAxis.Left_Stick_Y; currentTimeout = 0; return; }

                if (rightStickY < (-1 * percentDelta)) { mappingEntry.isReversed = true; mappingEntry.axis = ControllerAxis.Right_Stick_Y; currentTimeout = 0; return; }
                else if (rightStickY > percentDelta) { mappingEntry.isReversed = false; mappingEntry.axis = ControllerAxis.Right_Stick_Y; currentTimeout = 0; return; }

                if (leftTrigger < (-1 * percentDeltaTrigger)) { mappingEntry.isReversed = true; mappingEntry.axis = ControllerAxis.Left_Trigger_Pressure; currentTimeout = 0; return; }
                else if (leftTrigger > percentDeltaTrigger) { mappingEntry.isReversed = false; mappingEntry.axis = ControllerAxis.Left_Trigger_Pressure; currentTimeout = 0; return; }

                if (rightTrigger < (-1 * percentDeltaTrigger)) { mappingEntry.isReversed = true; mappingEntry.axis = ControllerAxis.Right_Trigger_Pressure; currentTimeout = 0; return; }
                else if (rightTrigger > percentDeltaTrigger) { mappingEntry.isReversed = false; mappingEntry.axis = ControllerAxis.Right_Trigger_Pressure; currentTimeout = 0; return; }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = (float)timeoutSeconds - ((pollCounter * SLEEP_TIME_POLLING) / MILLISECONDS_IN_SECOND);

                // Sleep
                Thread.Sleep(SLEEP_TIME_POLLING);
            }

            // Set current timeout (suppress compiler)
            currentTimeout = 0;
        }

        /// <summary>
        /// Waits for the user to press a button and retrieves the last pressed button. 
        /// Accepts any axis as input. Returns the read-in axis.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        public void RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap)
        {
            // Retrieve the object type of the controller.
            Type controllerType = Controller.GetType();

            // If it's a DirectInput controller.
            if (controllerType == typeof(DInputController)) { DInputRemapButton(timeoutSeconds, out currentTimeout, ref buttonToMap); }
            else if (controllerType == typeof(XInputController)) { XInputRemapButton(timeoutSeconds, out currentTimeout, ref buttonToMap); }
            else { currentTimeout = 0; }
        }


        /// <summary>
        /// Remaps a DirectInput button to the controller button map struct.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        private void DInputRemapButton(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap)
        {
            // Cast Controller to DInput Controller
            DInputController dInputController = (DInputController)Controller;

            // Retrieve Joystick State
            JoystickState joystickState = dInputController.GetCurrentState();

            // Initialize Timeout
            int pollAttempts = (timeoutSeconds * MILLISECONDS_IN_SECOND) / SLEEP_TIME_POLLING;
            int pollCounter = 0;
            int percentDelta = (int)((DInputManager.AXIS_MAX_VALUE / 100.0F) * PERCENTAGE_AXIS_DELTA);

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
                        ButtonMapping buttonMapping = Controller.ButtonMapping;

                        // Assign requested button.
                        buttonToMap = (byte)x;

                        // Reassign button mapping.
                        Controller.ButtonMapping = buttonMapping;

                        // Set timeout to 0
                        currentTimeout = 0;

                        // Return
                        return;
                    }
                }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = (float)timeoutSeconds - ((pollCounter * SLEEP_TIME_POLLING) / MILLISECONDS_IN_SECOND);

                // Sleep
                Thread.Sleep(SLEEP_TIME_POLLING);
            }

            // Assign the current timeout.
            currentTimeout = 0;
        }

        /// <summary>
        /// Remaps a XInput button mapped to a buton struct to the controller button map struct.
        /// </summary>
        /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
        /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
        /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
        private void XInputRemapButton(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap)
        {
            // Cast Controller to DInput Controller
            XInputController xInputController = (XInputController)Controller;

            // Retrieve Joystick State
            bool[] buttonState = xInputController.GetButtons();

            // Initialize Timeout
            int pollAttempts = (timeoutSeconds * MILLISECONDS_IN_SECOND) / SLEEP_TIME_POLLING;
            int pollCounter = 0;
            int percentDelta = (int)((DInputManager.AXIS_MAX_VALUE / 100.0F) * PERCENTAGE_AXIS_DELTA);

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
                        ButtonMapping buttonMapping = Controller.ButtonMapping;

                        // Assign requested button.
                        buttonToMap = (byte)x;

                        // Reassign button mapping.
                        Controller.ButtonMapping = buttonMapping;

                        // Set timeout to 0
                        currentTimeout = 0;

                        // Return
                        return;
                    }
                }

                // Increase counter, calculate new time left.
                pollCounter += 1;
                currentTimeout = (float)timeoutSeconds - ((pollCounter * SLEEP_TIME_POLLING) / MILLISECONDS_IN_SECOND);

                // Sleep
                Thread.Sleep(SLEEP_TIME_POLLING);
            }

            // Assign the current timeout.
            currentTimeout = 0;
        }
    }
}

