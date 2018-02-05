using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reloaded.Input
{
    /// <summary>
    /// The Hotplugger class provides a class which acts as a message only window, intercepting controller
    /// connections and disconnections. A specific delegate is ran upon device connection and disconnection.
    /// Within the loader, all controllers and input devices are re-evaluated upon device removal or insertion. Their configurations reloaded (if necessary).
    /// </summary>
    class Hotplugger : NativeWindow
    {
        /// <summary>
        /// Message type which is sent to a window (accessible in C# via Message.Msg).
        /// Notifies an application of a change to the hardware configuration of a device or the computer.
        /// </summary>
        public const int WM_DEVICECHANGE = 0x0219;

        /// <summary>
        /// The system broadcasts the DBT_DEVICEARRIVAL device event when a device or piece of media has been inserted and becomes available. (Controller Connect)
        /// </summary>
        public const int DBT_DEVICEARRIVAL = 0x8000;

        /// <summary>
        /// The system broadcasts the DBT_DEVICEREMOVECOMPLETE device event when a device or piece of media has been physically removed.
        /// </summary>
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        /// <summary>
        /// The constant for HWND_MESSAGE, specify this is a parent to a window to make it a message-only window.
        /// </summary>
        const long HWND_MESSAGE = -3;

        /// <summary>
        /// Provides a delegate signature for controller configuration re-parsing.
        /// </summary>
        public delegate void GetConnectedControllersDelegate();

        /// <summary>
        /// Delegate used when a controller is connected or disconnected.
        /// </summary>
        private GetConnectedControllersDelegate controllerConnectDelegate;

        /// <summary>
        /// Hosts an instance of the deviceNotification class which sends a message to this window whenever a controller device is inserted or removed.
        /// </summary>
        private DeviceNotification deviceNotificationDispatcher;

        /// <summary>
        /// Constructor for the Hotplugger class. Requires an initial method to which send messages when a device is
        /// connected or disconencted.
        /// </summary>
        /// <param name="methodDelegate"></param>
        public Hotplugger(Delegate methodDelegate)
        {
            // Create a new CreateParameters object.
            CreateParams cp = new CreateParams();

            // Specify HWND_MESSAGE in the hwndParent parameter such that the window only receives messages, no rendering, etc.
            cp.Parent = (IntPtr)(HWND_MESSAGE);

            // Create the handle for the message only window.
            this.CreateHandle(cp);

            // Adds the specific delegate such that it is ran upon connecting a controller.
            controllerConnectDelegate += (GetConnectedControllersDelegate)methodDelegate;

            // Register this window to receive controller connect and disconnect notifications.
            deviceNotificationDispatcher = new DeviceNotification(this.Handle, false);
        }

        /// <summary>
        /// Removes a specific passed in delegate instance if it exists/is currently assigned.
        /// </summary>
        public void RemoveDelegate(Delegate methodDelegate)
        {
            // Get pointer to function
            controllerConnectDelegate -= (GetConnectedControllersDelegate)methodDelegate;
        }

        /// <summary>
        /// Handler of Window Messages, calls the delegate if the message sent to the window is a change in device.
        /// </summary>
        protected override void WndProc(ref Message message)
        {
            // If the message is a device change event.
            if (message.Msg == WM_DEVICECHANGE)
            {
                // Switch on the message code.
                switch ((int)message.WParam)
                {
                    // Upon Connecting to a device.
                    case DBT_DEVICEARRIVAL:
                        controllerConnectDelegate();
                        break;

                    // Upon removing a device.
                    case DBT_DEVICEREMOVECOMPLETE:
                        controllerConnectDelegate();
                        break;
                }
            }

            // Call the original window message procedure for the window. 
            base.WndProc(ref message);
        }

        /// <summary>
        /// Allows for listening of individual device changes such as the change in connected controllers.
        /// Registers device notifications to be directed towards the read-only window.
        /// </summary>
        class DeviceNotification
        {
            /// <summary>
            /// The device type, which determines the event-specific information that follows the first three members.
            /// </summary>
            private const int DBT_DEVTYP_DEVICEINTERFACE = 5;

            //https://msdn.microsoft.com/en-us/library/aa363431(v=vs.85).aspx
            /// <summary>
            /// Notifies the recipient of device interface events for all device interface classes (classGuid is ignored).
            /// Setting this triggers events and reloads for non-USB devices.
            /// </summary>
            private const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

            /// <summary>
            /// The GUID_DEVINTERFACE_USB_DEVICE device interface class is defined for USB devices that are attached to a USB hub.
            /// </summary>
            private readonly Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

            /// <summary>
            /// Specifies a handle to our newly registered device notification to be sent to the specified window.
            /// </summary>
            private IntPtr notificationHandle; 

            /// <summary>
            /// Constructor. Creates a message-only window to receive notifications when devices are plugged or unplugged and calls a function specified by a delegate.
            /// </summary>
            /// <param name="windowHandle">Handle to the window receiving notifications.</param>
            /// <param name="usbOnly">true to filter to USB devices only, false to be notified for all devices.</param>
            public DeviceNotification(IntPtr windowHandle, bool usbOnly)
            {
                // Define the filter for device notifications which contains information about a class of devices.
                var deviceBroadcastInterface = new DEV_BROADCAST_DEVICEINTERFACE
                {
                    deviceType = DBT_DEVTYP_DEVICEINTERFACE,
                    reserved = 0,
                    classGuid = GUID_DEVINTERFACE_USB_DEVICE,
                    name = 0
                };

                // Retrieve the size for this filter struct.
                deviceBroadcastInterface.size = Marshal.SizeOf(deviceBroadcastInterface);

                // Allocate a buffer in unmanaged memory with the size of the filter.
                IntPtr buffer = Marshal.AllocHGlobal(deviceBroadcastInterface.size);

                // Marshal the DEV_BROADCAST_DEVICEINTERFACE structure into unmanaged memory.
                Marshal.StructureToPtr(deviceBroadcastInterface, buffer, true);

                // Register the notification handle for the message only window to receive controller connect/disconnect notifications.
                notificationHandle = RegisterDeviceNotification(windowHandle, buffer, usbOnly ? 0 : DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
            }

            /// <summary>
            /// Unregisters the individual window for device notifications.
            /// </summary>
            public void UnregisterDeviceNotification()
            {
                UnregisterDeviceNotification(notificationHandle);
            }

            /// <summary>
            /// Registers the device or type of device for which a window will receive notifications.
            /// </summary>
            /// <param name="recipient">Sets the handle to the window or service to receive the notification/</param>
            /// <param name="notificationFilter">A pointer to a block of data that specifies the type of device for which notifications should be sent. </param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

            /// <summary>
            /// Closes the specified device notification handle.
            /// </summary>
            /// <param name="handle">Handle for the notification returned from RegisterDeviceNotification</param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            private static extern bool UnregisterDeviceNotification(IntPtr handle);

            /// <summary>
            /// Struct which represents a filter for device notifications which contains information about a class of devices.
            /// </summary>
            /// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa363244(v=vs.85).aspx"/>
            [StructLayout(LayoutKind.Sequential)]
            private struct DEV_BROADCAST_DEVICEINTERFACE
            {
                /// <summary>
                /// The size of this structure, in bytes. This is the size of the members plus the actual length of the dbcc_name string (null terminator accounted).
                /// </summary>
                internal int size;

                /// <summary>
                /// Set to DBT_DEVTYP_DEVICEINTERFACE. The device type, which determines the event-specific information that follows the first three members.
                /// </summary>
                internal int deviceType;

                /// <summary>
                /// Do not use.
                /// </summary>
                internal int reserved;

                /// <summary>
                /// The GUID for the interface device class.
                /// </summary>
                internal Guid classGuid;

                /// <summary>
                /// Device name (ignore).
                /// </summary>
                internal short name;
            }
        }

    }
}
