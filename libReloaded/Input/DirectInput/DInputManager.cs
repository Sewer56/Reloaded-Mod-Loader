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
using System.IO;
using System.Threading;
using SharpDX.DirectInput;

namespace Reloaded.Input.DirectInput
{
    /// <summary>
    /// Loads and initializes all of the mod loader's DirectInput controllers.
    /// </summary>
    public class DInputManager
    {
        /// <summary>
        /// Represents the maximum set value that is to be returned from a controller.
        /// </summary>
        public static int AxisMaxValue = 10000;

        /// <summary>
        /// Represents the minimum set value that is to be returned from a controller.
        /// </summary>
        public static int AxisMinValue = -10000;

        /// <summary>
        /// Defines the factor by which the range of the values for the trigger (max - min)
        /// is scaled.
        /// </summary>
        public static float TriggerScaleFactor = 0.5F;

        /// <summary>
        /// The name of a file that is temporarily dumped onto the game directory which signifies that
        /// a controller is currently being acquired via DirectInput. This should be consistent between mods
        /// as multiple mods trying to acquire DInput devices at once could lead to deadlocks.  
        /// </summary>
        public static string ControllerAcquireFilenameFlag = "Controller_Acquire.txt";

        /// <summary>
        /// Stores a list of all DirectInput controller devices such as Mice, Keyboards
        /// and Joysticks used within the mod loader.
        /// </summary>
        private List<DInputController> _dInputControllers;

        /// <summary>
        /// Declare the directinput adapter used for acquiring directinput devices.
        /// </summary>
        private SharpDX.DirectInput.DirectInput _directInputAdapter;

        /// <summary>
        /// Loads and initializes all of the mod loader's DirectInput controllers.
        /// Exposes various method for obtaining input from a set of DirectInput controllers.
        /// </summary>
        public DInputManager()
        {
            // Sets up the DInput Manager
            AcquireDevices();
        }

        /// <summary>
        /// Loads and initializes all of the mod loader's DirectInput controllers.
        /// Exposes various method for obtaining input from a set of DirectInput controllers.
        /// </summary>
        public void AcquireDevices()
        {
            // Creates a lock disallowing simultaneous controller acquisitions between mods.
            CreateAcquisitionLock();

            // Retrieve and Setup Connected Devices
            GetConnectedControllers();

            // Removes the Acquisition Lock.
            DestroyAcquisitionLock();
        }

        /// <summary>
        /// Retrieves a list of currently available connected DirectInput devices.
        /// </summary>
        public List<DInputController> RetrieveDevices()
        {
            return _dInputControllers;
        }

        /// <summary>
        /// Waits if an existing lock exists and once it is removed, create a new one to prevent
        /// potential deadlocks as multiple mods may try to simultaneously acquire the same
        /// input devices. This is a fail-safe as new mods should contact back the mod-loader server
        /// to inform the server that a mod has finished loading.
        /// </summary>
        private void CreateAcquisitionLock()
        {
            // Wait for potential lock.
            // 1 second timeout.
            int timeout = 0;
            while (File.Exists(ControllerAcquireFilenameFlag))
            {
                timeout += 1;
                if (timeout == 60) break;
                Thread.Sleep(16);
            }

            // Create lock.
            File.Create(ControllerAcquireFilenameFlag).Close();
        }

        /// <summary>
        /// Removes the acquisition file lock such that another mod utilizing DirectInput may 
        /// acquire controller input. See note in CreateAcquitionLock();
        /// </summary>
        private void DestroyAcquisitionLock()
        {
            // Unlock
            File.Delete(ControllerAcquireFilenameFlag);
        }

        /// <summary>
        /// Acquires, finds and evaluates all of the currently available joysticks. keyboards and
        /// other valid input devices.
        /// </summary>
        private void GetConnectedControllers()
        {
            // Instantiate the DirectInput adapter.
            _directInputAdapter = new SharpDX.DirectInput.DirectInput();

            // Allocate a list of controllers.
            _dInputControllers = new List<DInputController>();

            // Allocate a list of device instances.
            List<DeviceInstance> dInputDevices = new List<DeviceInstance>();

            // Acquire all DInput devices.
            dInputDevices.AddRange(_directInputAdapter.GetDevices(DeviceClass.All, DeviceEnumerationFlags.AttachedOnly));

            // Acquire and initialize each device.
            foreach (DeviceInstance dInputDevice in dInputDevices)
            {
                // Filter devices to initialize by type.
                if (dInputDevice.Type == DeviceType.Joystick)
                    _dInputControllers.Add(SetupController(dInputDevice));

                else if (dInputDevice.Type == DeviceType.Gamepad)
                    _dInputControllers.Add(SetupController(dInputDevice));

                else if (dInputDevice.Type == DeviceType.Keyboard)
                    _dInputControllers.Add(SetupController(dInputDevice));

                else if (dInputDevice.Type == DeviceType.Mouse)
                    _dInputControllers.Add(SetupController(dInputDevice));
            }
        }

        /// <summary>
        /// Instantiates and sets up an individual controller and/or input device with the
        /// DInputController class in place. Returns an instantiated controller instance.
        /// <param name="dInputDevice">Configures a DirectInput device with the appropriate axis ranges, 
        /// absolute/relative axis settings and acquires the DirectInput devices for our usage within Reloaded Mod Loader.</param>
        /// </summary>
        private DInputController SetupController(DeviceInstance dInputDevice)
        {
            // Initialize Joystick/Controller
            DInputController dInputController = new DInputController(_directInputAdapter, dInputDevice.InstanceGuid);

            // If the device is a mouse, set the axis mode to relative.
            if (dInputController.Information.Type == DeviceType.Mouse)
                dInputController.Properties.AxisMode = DeviceAxisMode.Relative;

            // Acquire the DInput Device
            dInputController.Acquire();

            // For each Device Object/Controller/Input type in the Direct Input Devices. 
            // If it contains an axis, set the range of the axis to AXIS_MIN_VALUE, AXIS_MAX_VALUE
            foreach (DeviceObjectInstance deviceObject in dInputController.GetObjects())

            // Check if the object flags contain axis bits.
            if (deviceObject.ObjectId.Flags.HasFlag(DeviceObjectTypeFlags.AbsoluteAxis))
                dInputController.GetObjectPropertiesById(deviceObject.ObjectId).Range = new InputRange(AxisMinValue, AxisMaxValue);

            // Return the DirectInput Device.
            return dInputController;
        }
    }
}
