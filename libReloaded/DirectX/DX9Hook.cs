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
using System.Windows.Forms;
using Reloaded.DirectX.Definitions;
using Reloaded.Process.Functions.Utilities;
using SharpDX.Direct3D9;

namespace Reloaded.DirectX
{
    /// <summary>
    /// Provides a wrapper for easy DirectX 9 function hooking.
    /// </summary>
    public class DX9Hook
    {
        /// <summary>
        /// A virtual function table containing the individual DirectX functions which may be hooked.
        /// See <see cref="Direct3DDevice9"/> for an enumerable lits of functions that can be hooked.
        /// </summary>
        public VirtualFunctionTable DirectXFunctions;

        /// <summary>
        /// Boolean declaring whether Direct3D 9Ex is supported for this game or process.
        /// </summary>
        public bool SupportsDirect3D9Ex;

        /// <summary>
        /// Constructor for the hooking class.
        /// Creates the Direct3D9/Direct3D9Ex devices inside the function to get their
        /// native pointer for hooking purposes.
        /// </summary>
        public DX9Hook()
        {
            // Obtain the pointer to the IDirect3DDevice9 instance
            // by creating our own blank windows form and creating a IDirect3DDevice9
            // targeting that form. The returned device should be the same one as used by the game.
            using (Direct3D direct3D = new Direct3D())
            using (Form renderForm = new Form())
            using (Device device = new Device(direct3D, 0, DeviceType.NullReference, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1, DeviceWindowHandle = renderForm.Handle }))
            {
                DirectXFunctions = new VirtualFunctionTable(device.NativePointer, Enum.GetNames(typeof(Direct3DDevice9)).Length);
            }

            // Obtain the pointer to the IDirect3DDevice9Ex instance
            // by creating our own blank windows form and creating a IDirect3DDevice9Ex
            // targeting that form. The returned device should be the same one as used by the game.
            using (Direct3DEx direct3D = new Direct3DEx())
            using (var renderForm = new Form())
            using (DeviceEx device = new DeviceEx(direct3D, 0, DeviceType.NullReference, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1, DeviceWindowHandle = renderForm.Handle }))
            {
                try
                {
                    DirectXFunctions.TableEntries.AddRange(VirtualFunctionTable.GetObjectVTableAddresses(device.NativePointer, Enum.GetNames(typeof(Direct3DDevice9Ex)).Length));
                    SupportsDirect3D9Ex = true;
                }
                catch { SupportsDirect3D9Ex = false; }
            }
        }
    }
}
