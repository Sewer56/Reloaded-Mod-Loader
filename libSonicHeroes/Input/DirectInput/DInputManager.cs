using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SonicHeroes.Input.DirectInput
{
    /// <summary>
    /// Loads and initializes all of the mod loader's DirectInput controllers.
    /// </summary>
    public class DInputManager
    {
        /// <summary>
        /// Represents the maximum set value that is to be returned from a controller.
        /// </summary>
        public static int AXIS_MAX_VALUE = 10000;

        /// <summary>
        /// Represents the minimum set value that is to be returned from a controller.
        /// </summary>
        public static int AXIS_MIN_VALUE = -10000;

        /// <summary>
        /// Represents the maximum value of the axis as returned to the modder/user.
        /// </summary>
        public static float AXIS_MAX_VALUE_F = 100;

        /// <summary>
        /// Represents the minimum value of the axis as returned to the modder/user.
        /// </summary>
        public static float AXIS_MIN_VALUE_F = -100;

        /// <summary>
        /// Defines the factor by which the range of the values for the trigger (max - min)
        /// is scaled.
        /// </summary>
        public static float TRIGGER_SCALE_FACTOR = 0.5F;

        /// <summary>
        /// The name of a file that is temporarily dumped onto the game directory which signifies that
        /// a controller is currently being acquired via DirectInput. This should be consistent between mods
        /// as multiple mods trying to acquire DInput devices at once could lead to deadlocks.  
        /// </summary>
        public static string CONTROLLER_ACQUIRE_FILENAME_FLAG = "Controller_Acquire.txt";

        /// <summary>
        /// Declare the directinput adapter used for acquiring directinput devices.
        /// </summary>
        private SharpDX.DirectInput.DirectInput directInputAdapter;

        /// <summary>
        /// Stores a list of all DirectInput controller devices such as Mice, Keyboards
        /// and Joysticks used within the mod loader.
        /// </summary>
        private List<DInputController> dInputControllers;

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
            return dInputControllers;
        }

        /// <summary>
        /// Waits if an existing lock exists and once it is removed, create a new one to prevent
        /// potential deadlocks as multiple mods may try to simultaneously acquire the same
        /// input devices. This is a fail-safe as new mods should contact back the mod-loader server
        /// to inform the server that a mod has finished loading.
        /// TODO: Update this as the mod loader server is updated.
        /// </summary>
        private void CreateAcquisitionLock()
        {
            // Wait for potential lock.
            while (File.Exists(CONTROLLER_ACQUIRE_FILENAME_FLAG)) { Thread.Sleep(16); }

            // Create lock.
            File.Create(CONTROLLER_ACQUIRE_FILENAME_FLAG).Close();
        }

        /// <summary>
        /// Removes the acquisition file lock such that another mod utilizing DirectInput may 
        /// acquire controller input. See note in CreateAcquitionLock();
        /// </summary>
        private void DestroyAcquisitionLock()
        {
            // Unlock
            File.Delete(CONTROLLER_ACQUIRE_FILENAME_FLAG);
        }

        /// <summary>
        /// Acquires, finds and evaluates all of the currently available joysticks. keyboards and
        /// other valid input devices.
        /// </summary>
        private void GetConnectedControllers()
        {
            // Instantiate the DirectInput adapter.
            directInputAdapter = new SharpDX.DirectInput.DirectInput();

            // Allocate a list of controllers.
            dInputControllers = new List<DInputController>();

            // Allocate a list of device instances.
            List<DeviceInstance> DInputDevices = new List<DeviceInstance>();

            // Acquire all Joysticks, Keyboards, Mice
            DInputDevices.AddRange(directInputAdapter.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly));
            DInputDevices.AddRange(directInputAdapter.GetDevices(DeviceClass.Keyboard, DeviceEnumerationFlags.AttachedOnly));
            DInputDevices.AddRange(directInputAdapter.GetDevices(DeviceType.Mouse, DeviceEnumerationFlags.AttachedOnly));

            // Acquire and initialize each device.
            foreach (DeviceInstance DInputDevice in DInputDevices) {
                dInputControllers.Add(SetupController(DInputDevice));
            }
        }

        /// <summary>
        /// Instantiates and sets up an individual controller and/or input device with the
        /// DInputController class in place. Returns an instantiated controller instance.
        /// </summary>
        private DInputController SetupController(DeviceInstance DInputDevice)
        {
            // Initialize Joystick/Controller
            DInputController dInputController = new DInputController(directInputAdapter, DInputDevice.InstanceGuid);

            // Acquire the DInput Device
            dInputController.Acquire();

            // For each Device Object/Controller/Input type in the Direct Input Devices. 
            // If it contains an axis, set the range of the axis to AXIS_MIN_VALUE, AXIS_MAX_VALUE
            foreach (DeviceObjectInstance deviceObject in dInputController.GetObjects())
            {
                // Check if the object flags contain axis.
                if (deviceObject.ObjectId.Flags.HasFlag(DeviceObjectTypeFlags.Axis))
                {
                    // Set the range of the axis as defined in the class header.
                    dInputController.GetObjectPropertiesById(deviceObject.ObjectId).Range = new SharpDX.DirectInput.InputRange(AXIS_MIN_VALUE, AXIS_MAX_VALUE);
                }
            }

            // Return the DirectInput Device.
            return dInputController;
        }
    }
}
