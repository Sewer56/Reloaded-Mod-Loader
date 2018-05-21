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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Reloaded.DirectX;
using Reloaded.DirectX.Definitions;
using Reloaded.Process.Functions.Utilities;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Functions.X64Hooking;
using Reloaded.Process.Functions.X86Functions;
using Reloaded.Process.Functions.X86Hooking;
using SharpDX.DXGI;

namespace Reloaded.Overlay.Internal
{
    /// <summary>
    /// The <see cref="DX11Overlay"/> class provides support for drawing of internal overlays using Direct2D
    /// through the hooking of DX11 functions within the target proces. It allows you to easily draw your own overlay over the game
    /// displaying various pieces of information. If you want to get directly hands on instead without the abstraction, see <see cref="DX11Hook"/>.
    /// </summary>
    public class DX11Overlay
    {
        /// <summary>
        /// A copy of the delegate pointed to your own method used for rendering
        /// of your own 2D elements over the game content.
        /// </summary>
        public FunctionHook<DXGISwapChain_PresentDelegate> PresentHook { get; private set; }

        /// <summary>
        /// A copy of the delegate pointed to your own method used executed when
        /// the resolution, fullscreen/windowed mode or other state changes.
        /// </summary>
        public FunctionHook<DXGISwapChain_ResizeTargetDelegate> ResizeTargetHook { get; private set; }

        /// <summary>
        /// A copy of the delegate pointed to your own method used for rendering
        /// of your own 2D elements over the game content.
        /// </summary>
        public X64FunctionHook<DXGISwapChain_PresentDelegate> PresentHook64 { get; private set; }

        /// <summary>
        /// A copy of the delegate pointed to your own method used executed when
        /// the resolution, fullscreen/windowed mode or other state changes.
        /// </summary>
        public X64FunctionHook<DXGISwapChain_ResizeTargetDelegate> ResizeTargetHook64 { get; private set; }

        /// <summary>
        /// An instance of the <see cref="DX11Hook"/> class allowing us to easily manage
        /// and hook as well as unhook various DirectX11 functions.
        /// </summary>
        public DX11Hook DirectX11Hook { get; private set; }

        /// <summary>
        /// Defines the IDXGISwapChain.Present function, used to show the rendered image right to the user.
        /// </summary>
        /// <param name="swapChainPtr">The pointer to the actual swapchain, `this` object.</param>
        /// <param name="syncInterval">An integer that specifies how to synchronize presentation of a frame with the vertical blank.</param>
        /// <param name="flags">An integer value that contains swap-chain presentation options. These options are defined by the DXGI_PRESENT constants.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int DXGISwapChain_PresentDelegate(IntPtr swapChainPtr, int syncInterval, PresentFlags flags);

        /// <summary>
        /// Defines the IDXGISwapChain.ResizeTarget function, called when the game window is resized or the user switches to fullscreen etc.
        /// </summary>
        /// <param name="swapChainPtr">The pointer to the actual swapchain, `this` object.</param>
        /// <param name="newTargetParameters">Defines the details of the new display mode.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int DXGISwapChain_ResizeTargetDelegate(IntPtr swapChainPtr, ref ModeDescription newTargetParameters);

        /// <summary>
        /// Instantiates the DirectX overlay, by first finding the applicable
        /// version of DirectX for the application and then finding the individual
        /// details. For more details, see <see cref="DX11Overlay"/>
        /// 
        /// Note: The delegates you will need to call the original function are members of this class, see <see cref="PresentHook"/> and <see cref="ResizeTargetHook"/>
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="DXGIPresentDelegate">
        ///     A delegate type to use for DirectX rendering. The delegate type should
        ///     contain an appropriate DirectX <see cref="DXGISwapChain_PresentDelegate"/>
        ///     object for drawing overlays. 
        /// </param>
        /// <param name="DXGIResizeTargetDelegate">
        ///     A delegate or function of type of <see cref="DXGISwapChain_ResizeTargetDelegate"/> to call when DXGI Buffer 
        ///     commits a resolution change or windowed/fullscreen change.
        /// </param>
        /// <remarks>The delegates you will need to call the original function are members of this class, see <see cref="PresentHook"/> and <see cref="ResizeTargetHook"/></remarks>
        public static async Task<DX11Overlay> CreateDirectXOverlay(DXGISwapChain_PresentDelegate DXGIPresentDelegate, DXGISwapChain_ResizeTargetDelegate DXGIResizeTargetDelegate)
        {
            return await CreateDirectXOverlay(DXGIPresentDelegate, DXGIResizeTargetDelegate, 0);
        }

        /// <summary>
        /// Instantiates the DirectX overlay, by first finding the applicable
        /// version of DirectX for the application and then finding the individual
        /// details. For more details, see <see cref="DX11Overlay"/>
        /// 
        /// Note: The delegates you will need to call the original function are members of this class, see <see cref="PresentHook"/> and <see cref="ResizeTargetHook"/>
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="DXGIPresentDelegate">
        ///     A delegate type to use for DirectX rendering. The delegate type should
        ///     contain an appropriate DirectX <see cref="DXGISwapChain_PresentDelegate"/>
        ///     object for drawing overlays. 
        /// </param>
        /// <param name="DXGIResizeTargetDelegate">
        ///     A delegate or function of type of <see cref="DXGISwapChain_ResizeTargetDelegate"/> to call when DXGI Buffer 
        ///     commits a resolution change or windowed/fullscreen change.
        /// </param>
        /// <param name="hookDelay">
        ///     Specifies the amount of time to wait until the hook is instantiation begins.
        ///     Some games are known to crash if DirectX is hooked too early.
        /// </param>
        /// <remarks>The delegates you will need to call the original function are members of this class, see <see cref="PresentHook"/> and <see cref="ResizeTargetHook"/></remarks>
        public static async Task<DX11Overlay> CreateDirectXOverlay(DXGISwapChain_PresentDelegate DXGIPresentDelegate, DXGISwapChain_ResizeTargetDelegate DXGIResizeTargetDelegate, int hookDelay)
        {
            // Wait the hook delay.
            await Task.Delay(hookDelay);

            // Create a new self-object.
            DX11Overlay dx11Overlay = new DX11Overlay();

            // Wait for DirectX
            Direct3DVersion direct3DVersion = await DXHookCommon.GetDirectXVersion();

            // Return nothing if not D3D9
            if (direct3DVersion != Direct3DVersion.Direct3D11 && direct3DVersion != Direct3DVersion.Direct3D11_1 &&
                direct3DVersion != Direct3DVersion.Direct3D11_3 && direct3DVersion != Direct3DVersion.Direct3D11_4)
            {
                Bindings.PrintError( 
                    "libReloaded Hooking: DirectX 11 module not found, the application is either not " +
                    "a DirectX 11 application or uses an unsupported version of DirectX.");

                return null;
            }

            // Instantiate DX9 hook
            dx11Overlay.DirectX11Hook = new DX11Hook();;

            // Obtain Virtual Function Table Entries
            VirtualFunctionTable.TableEntry presentTableEntry = dx11Overlay.DirectX11Hook.DXGISwapChainFunctions[(int)IDXGISwapChain.Present];
            VirtualFunctionTable.TableEntry resizeTableEntry = dx11Overlay.DirectX11Hook.DXGISwapChainFunctions[(int)IDXGISwapChain.ResizeTarget];

            // Hook relevant DirectX functions.
            if (IntPtr.Size == 4)
            {
                // X86
                dx11Overlay.PresentHook = FunctionHook<DXGISwapChain_PresentDelegate>.Create((long)presentTableEntry.FunctionPointer, DXGIPresentDelegate);
                dx11Overlay.ResizeTargetHook = FunctionHook<DXGISwapChain_ResizeTargetDelegate>.Create((long)resizeTableEntry.FunctionPointer, DXGIResizeTargetDelegate);
            }
            else if (IntPtr.Size == 8)
            {
                // X64
                dx11Overlay.PresentHook64 = X64FunctionHook<DXGISwapChain_PresentDelegate>.Create((long)presentTableEntry.FunctionPointer, DXGIPresentDelegate);
                dx11Overlay.ResizeTargetHook64 = X64FunctionHook<DXGISwapChain_ResizeTargetDelegate>.Create((long)resizeTableEntry.FunctionPointer, DXGIResizeTargetDelegate);
            }

            // Return our DX9Overlay
            return dx11Overlay;
        }

        /// <summary>
        /// Private constructor, please use factory method <see cref="CreateDirectXOverlay"/> instead.
        /// </summary>
        private DX11Overlay() { }
    }
}
