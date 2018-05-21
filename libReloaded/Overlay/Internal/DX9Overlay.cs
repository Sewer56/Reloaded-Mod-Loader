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
using Reloaded.DirectX;
using Reloaded.DirectX.Definitions;
using Reloaded.Process.Functions.Utilities;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Functions.X64Hooking;
using Reloaded.Process.Functions.X86Functions;
using Reloaded.Process.Functions.X86Hooking;
using SharpDX.Direct3D9;

namespace Reloaded.Overlay.Internal
{
    /// <summary>
    /// The <see cref="DX9Overlay"/> class provides support for each drawing of internal overlays
    /// through the hooking of DX9 functions within the target proces. It allows you to easily draw your own overlay over the game
    /// displaying various pieces of information. If you want to get directly hands on instead without the abstraction, see <see cref="DX9Hook"/>.
    /// </summary>
    public class DX9Overlay
    {
        /// <summary>
        /// A copy of the delegate pointed to your own method used for rendering
        /// of your own 2D elements over the game content.
        /// </summary>
        public FunctionHook<Direct3D9Device_EndSceneDelegate> EndSceneHook { get; private set; }

        /// <summary>
        /// A copy of the delegate pointed to your own method used for rendering
        /// of your own 2D elements over the game content.
        /// </summary>
        public X64FunctionHook<Direct3D9Device_EndSceneDelegate> EndSceneHook64 { get; private set; }

        /// <summary>
        /// A copy of the delegate pointed to your own method used executed when
        /// the resolution, fullscreen/windowed mode or other state changes.
        /// </summary>
        public FunctionHook<Direct3D9Device_ResetDelegate> ResetHook { get; private set; }

        /// <summary>
        /// A copy of the delegate pointed to your own method used executed when
        /// the resolution, fullscreen/windowed mode or other state changes.
        /// </summary>
        public X64FunctionHook<Direct3D9Device_ResetDelegate> ResetHook64 { get; private set; }

        /// <summary>
        /// An instance of the <see cref="DX9Hook"/> class allowing us to easily manage
        /// and hook as well as unhook various DirectX9 functions.
        /// </summary>
        public DX9Hook DirectX9Hook { get; private set; }

        /// <summary>
        /// Defines the IDirect3DDevice9.EndScene function, allowing us to render ontop of the DirectX instance.
        /// </summary>
        /// <param name="device">Pointer to the individual Direct3D9 device.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int Direct3D9Device_EndSceneDelegate(IntPtr device);

        /// <summary>
        /// Defines the IDirect3DDevice9.Reset function, called when the resolution or Windowed/Fullscreen state changes.
        /// changes.
        /// </summary>
        /// <param name="device">Pointer to the individual Direct3D9 device.</param>
        /// <param name="presentParameters">Pointer to a D3DPRESENT_PARAMETERS structure, describing the new presentation parameters.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int Direct3D9Device_ResetDelegate(IntPtr device, ref PresentParameters presentParameters);

        /// <summary>
        /// Instantiates the DirectX overlay, by first finding the applicable
        /// version of DirectX for the application and then finding the individual
        /// details. For more details, see <see cref="DX9Overlay"/>
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="renderDelegate">
        ///     A delegate type to use for DirectX rendering. The delegate type should
        ///     contain an appropriate DirectX <see cref="Direct3D9Device_EndSceneDelegate"/>
        ///     object for drawing overlays. 
        /// </param>
        /// <param name="resetDelegate">
        ///     A delegate or function of type of <see cref="Direct3D9Device_ResetDelegate"/> to call when D3D9 fires its Reset function, 
        ///     called on resolution change or windowed/fullscreen change - we can reset some things as well.
        /// </param>
        public static async Task<DX9Overlay> CreateDirectXOverlay(Direct3D9Device_EndSceneDelegate renderDelegate, Direct3D9Device_ResetDelegate resetDelegate)
        {
            return await CreateDirectXOverlay(renderDelegate, resetDelegate, 0);
        }


        /// <summary>
        /// Instantiates the DirectX overlay, by first finding the applicable
        /// version of DirectX for the application and then finding the individual
        /// details. For more details, see <see cref="DX9Overlay"/>
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="renderDelegate">
        ///     A delegate type to use for DirectX rendering. The delegate type should
        ///     contain an appropriate DirectX <see cref="Direct3D9Device_EndSceneDelegate"/>
        ///     object for drawing overlays. 
        /// </param>
        /// <param name="resetDelegate">
        ///     A delegate or function of type of <see cref="Direct3D9Device_ResetDelegate"/> to call when D3D9 fires its Reset function, 
        ///     called on resolution change or windowed/fullscreen change - we can reset some things as well.
        /// </param>
        /// <param name="hookDelay">
        ///     Specifies the amount of time to wait until the hook is instantiation begins.
        ///     Some games are known to crash if DirectX is hooked too early.
        /// </param>
        public static async Task<DX9Overlay> CreateDirectXOverlay(Direct3D9Device_EndSceneDelegate renderDelegate, Direct3D9Device_ResetDelegate resetDelegate, int hookDelay)
        {
            // Wait the hook delay.
            await Task.Delay(hookDelay);

            // Create a new self-object.
            DX9Overlay dx9Overlay = new DX9Overlay();

            // Wait for DirectX
            Direct3DVersion direct3DVersion = await DXHookCommon.GetDirectXVersion();

            // Return nothing if not D3D9
            if (direct3DVersion != Direct3DVersion.Direct3D9)
            {
                Bindings.PrintError( 
                    "libReloaded Hooking: DirectX 9 module not found, the application is either not" +
                    "a DirectX 9 application or uses an unsupported version of DirectX.");

                return null;
            }

            // Instantiate DX9 hook
            dx9Overlay.DirectX9Hook = new DX9Hook();

            // Obtain Virtual Function Table Entries
            VirtualFunctionTable.TableEntry endSceneTableEntry = dx9Overlay.DirectX9Hook.DirectXFunctions[(int)Direct3DDevice9.EndScene];
            VirtualFunctionTable.TableEntry resetTableEntry = dx9Overlay.DirectX9Hook.DirectXFunctions[(int)Direct3DDevice9.Reset];

            // Hook relevant DirectX functions.
            if (IntPtr.Size == 4)
            {
                // X86
                dx9Overlay.EndSceneHook = FunctionHook<Direct3D9Device_EndSceneDelegate>.Create((long)endSceneTableEntry.FunctionPointer, renderDelegate);
                dx9Overlay.ResetHook = FunctionHook<Direct3D9Device_ResetDelegate>.Create((long)resetTableEntry.FunctionPointer, resetDelegate);
            }
            else if (IntPtr.Size == 8)
            {
                // X64
                dx9Overlay.EndSceneHook64 = X64FunctionHook<Direct3D9Device_EndSceneDelegate>.Create((long)endSceneTableEntry.FunctionPointer, renderDelegate);
                dx9Overlay.ResetHook64 = X64FunctionHook<Direct3D9Device_ResetDelegate>.Create((long)resetTableEntry.FunctionPointer, resetDelegate);
            }

            // Return our DX9Overlay
            return dx9Overlay;
        }

        /// <summary>
        /// Private constructor, please use factory method <see cref="CreateDirectXOverlay"/> instead.
        /// </summary>
        private DX9Overlay() { }
    }
}
