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
using Reloaded.Process.X86Functions.Utilities;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace Reloaded.DirectX
{
    /// <summary>
    /// Provides an easy to use wrapper for DirectX11 and DXGI function hooking.
    /// </summary>
    public class DX11Hook
    {
        /// <summary>
        /// A virtual function table containing the individual DirectX11 Device functions which may be hooked.
        /// See <see cref="ID3D11Device"/> for an enumerable lits of functions that can be hooked.
        /// </summary>
        public VirtualFunctionTable DirectX11DeviceFunctions;

        /// <summary>
        /// A virtual function table containing the individual DXGI Swap Chain functions which may be hooked.
        /// See <see cref="IDXGISwapChain"/> for an enumerable lits of functions that can be hooked.
        /// </summary>
        public VirtualFunctionTable DXGISwapChainFunctions;

        /// <summary>
        /// Constructor for the DX11 hooking class.
        /// Creates the Direct3D11 device and DXGI swapchain inside the function to get their
        /// native pointer for hooking purposes.
        /// </summary>
        public DX11Hook()
        {
            /*
                Target: Obtain the pointer to the ID3D11Device and IDXGISwapChain instances
                by creating our own blank windows form and creating a ID3D11Device with Swapchain
                targeting that form. The returned device should be the same one as used by the game.
            */
            
            // Declare the Swapchain, Windows form, DX11 device.
            Form renderForm = new Form();
            SwapChain dxgiSwapChain;
            Device dx11Device;

            // Create the Device and SwapChain
            Device.CreateWithSwapChain (
                DriverType.Hardware,
                DeviceCreationFlags.BgraSupport,
                new SharpDX.DXGI.SwapChainDescription
                {
                    BufferCount = 1,
                    Flags = SharpDX.DXGI.SwapChainFlags.None,
                    IsWindowed = true,
                    ModeDescription = new SharpDX.DXGI.ModeDescription(100, 100, new Rational(60, 1), SharpDX.DXGI.Format.R8G8B8A8_UNorm),
                    OutputHandle = renderForm.Handle,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard,
                    Usage = SharpDX.DXGI.Usage.RenderTargetOutput
                },
                out dx11Device,
                out dxgiSwapChain
            );

            // Get our VTable function pointers.
            DirectX11DeviceFunctions = new VirtualFunctionTable(dx11Device.NativePointer, Enum.GetNames(typeof(ID3D11Device)).Length);
            DXGISwapChainFunctions = new VirtualFunctionTable(dxgiSwapChain.NativePointer, Enum.GetNames(typeof(IDXGISwapChain)).Length);

            // Dispose of the native objects.
            renderForm.Dispose();
            dxgiSwapChain.Dispose();
            dx11Device.Dispose();;
        }
    }
}
