﻿/*
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reloaded.Input.Modules
{
    /// <summary>
    /// The Hotplugger class provides a class which acts as a message only window, intercepting controller
    /// connections and disconnections. A specific delegate is ran upon device connection and disconnection.
    /// Within the loader, all controllers and input devices are re-evaluated upon device removal or insertion. Their configurations reloaded (if necessary).
    /// </summary>
    public class Hotplugger : NativeWindow, IDisposable
    {
        /// <summary>
        /// Provides a delegate signature for controller configuration re-parsing.
        /// </summary>
        public delegate void GetConnectedControllersDelegate();

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
        private const long HWND_MESSAGE = -3;

        /// <summary>
        /// Delegate used when a controller is connected or disconnected.
        /// </summary>
        private GetConnectedControllersDelegate _controllerConnectDelegate;

        /// <summary>
        /// Hosts an instance of the deviceNotification class which sends a message to this window whenever a controller device is inserted or removed.
        /// </summary>
        private readonly DeviceNotification _deviceNotificationDispatcher;

        /// <summary>
        /// Constructor for the Hotplugger class. Requires an initial method to which send messages when a device is
        /// connected or disconencted.
        /// </summary>
        /// <param name="methodDelegate">The method to be executed when a new controller is connected or disconnected.</param>
        public Hotplugger(Delegate methodDelegate)
        {
            // Create a new CreateParameters object.
            CreateParams cp = new CreateParams
            {
                // Specify HWND_MESSAGE in the hwndParent parameter such that the window only receives messages, no rendering, etc.
                Parent = (IntPtr) HWND_MESSAGE
            };
            
            // Create the handle for the message only window.
            CreateHandle(cp);

            // Adds the specific delegate such that it is ran upon connecting a controller.
            _controllerConnectDelegate += (GetConnectedControllersDelegate)methodDelegate;

            // Register this window to receive controller connect and disconnect notifications.
            _deviceNotificationDispatcher = new DeviceNotification(Handle, false);
        }

        /// <summary>
        /// Removes a specific passed in delegate instance if it exists/is currently assigned.
        /// </summary>
        public void RemoveDelegate(Delegate methodDelegate)
        {
            // Get pointer to function
            _controllerConnectDelegate -= (GetConnectedControllersDelegate)methodDelegate;
        }

        /// <summary>
        /// Handler of Window Messages, calls the delegate if the message sent to the window is a change in device.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // If the message is a device change event.
            if (m.Msg == WM_DEVICECHANGE)
                switch ((int)m.WParam)
                {
                    // Upon Connecting to a device.
                    case DBT_DEVICEARRIVAL:
                        _controllerConnectDelegate();
                        break;

                    // Upon removing a device.
                    case DBT_DEVICEREMOVECOMPLETE:
                        _controllerConnectDelegate();
                        break;
                }

            // Call the original window message procedure for the window. 
            base.WndProc(ref m);
        }

        /// <summary>
        /// Allows for listening of individual device changes such as the change in connected controllers.
        /// Registers device notifications to be directed towards the read-only window.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class DeviceNotification : IDisposable
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
            private readonly IntPtr _notificationHandle;

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
                deviceBroadcastInterface.size = Unsafe.SizeOf<DEV_BROADCAST_DEVICEINTERFACE>();

                // Allocate a buffer in unmanaged memory with the size of the filter.
                IntPtr buffer = Marshal.AllocHGlobal(deviceBroadcastInterface.size);

                // Marshal the DEV_BROADCAST_DEVICEINTERFACE structure into unmanaged memory.
                unsafe
                {
                    Unsafe.Write((void*)buffer, deviceBroadcastInterface);
                }

                // Register the notification handle for the message only window to receive controller connect/disconnect notifications.
                _notificationHandle = RegisterDeviceNotification(windowHandle, buffer, usbOnly ? 0 : DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
            }

            /// <summary>
            /// Unregisters the individual window for device notifications.
            /// </summary>
            public void UnregisterDeviceNotification()
            {
                UnregisterDeviceNotification(_notificationHandle);
            }

            /// <summary>
            /// Registers the device or type of device for which a window will receive notifications.
            /// </summary>
            /// <param name="recipient">Sets the handle to the window or service to receive the notification/</param>
            /// <param name="notificationFilter">A pointer to a block of data that specifies the type of device for which notifications should be sent. </param>
            /// <param name="flags">See MSDN.</param>
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

            protected virtual void Dispose( bool disposing )
            {
                if ( !disposing )
                    UnregisterDeviceNotification();
            }

            public void Dispose()
            {
                Dispose( true );
                GC.SuppressFinalize( this );
            }
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( disposing )
            {
                _deviceNotificationDispatcher?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
    }
}
