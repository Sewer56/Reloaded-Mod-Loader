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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Reloaded.DirectX;
using Reloaded.DirectX.Definitions;
using Reloaded.Process.X86Functions;
using Reloaded.Process.X86Functions.CustomFunctionFactory;
using Reloaded.Process.X86Functions.Utilities;
using Reloaded.Process.X86Hooking;
using SharpDX.DXGI;

namespace Reloaded.Overlay
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
        /// An instance of the <see cref="DX11Hook"/> class allowing us to easily manage
        /// and hook as well as unhook various DirectX11 functions.
        /// </summary>
        public DX11Hook DirectX11Hook { get; private set; }

        /// <summary>
        /// Defines the delegate type which fires the user's own method 
        /// used for rendering using a Direct3D11 device.
        /// </summary>
        private DXGISwapChain_PresentDelegate direct2DRenderMethod;

        /*
         * Commented out is are remainments of (failed) attempts to bring over Direct2D
         * rendering into Direct3D games universally. If you have sufficient experience
         * in working with DirectX, contributions would be appreciated.
         */

        /// <summary>
        /// Flag indicating whether Direct2D has been setup for user rendering for the first time.
        /// </summary>
        //private bool _direct2DSetup = false;

        /// <summary>
        /// Defines the delegate type which fires the user's own method 
        /// used for rendering onto a Direct2D surface (or the screen rather).
        /// </summary>
        //private Direct2DRenderDelegate direct2DRenderMethod;

        /// <summary>
        /// The individual Direct2D render target used for rendering elements
        /// onto the screen of the user.
        /// </summary>
        //private RenderTarget direct2DRenderTarget;
        //private bool isDrawing;

        /// <summary>
        /// Delegate type which describes a method that accepts a RenderTarget for Direct2D rendering onto the game.
        /// </summary>
        /// <param name="renderTarget">The render target to which the user may draw individual elements.</param>
        //public delegate void Direct2DRenderDelegate(RenderTarget renderTarget);

        /// <summary>
        /// Defines the IDXGISwapChain.Present function, used to show the rendered image right to the user.
        /// </summary>
        /// <param name="swapChainPtr">The pointer to the actual swapchain, `this` object.</param>
        /// <param name="syncInterval">An integer that specifies how to synchronize presentation of a frame with the vertical blank.</param>
        /// <param name="flags">An integer value that contains swap-chain presentation options. These options are defined by the DXGI_PRESENT constants.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int DXGISwapChain_PresentDelegate(IntPtr swapChainPtr, int syncInterval, PresentFlags flags);

        /// <summary>
        /// Defines the IDXGISwapChain.ResizeTarget function, called when the game window is resized or the user switches to fullscreen etc.
        /// </summary>
        /// <param name="swapChainPtr">The pointer to the actual swapchain, `this` object.</param>
        /// <param name="newTargetParameters">Defines the details of the new display mode.</param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
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
            Direct3DVersion direct3DVersion = await DXHookCommon.DetermineDirectXVersion();

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
            dx11Overlay.PresentHook = new FunctionHook<DXGISwapChain_PresentDelegate>((long)presentTableEntry.FunctionPointer, DXGIPresentDelegate);
            dx11Overlay.ResizeTargetHook = new FunctionHook<DXGISwapChain_ResizeTargetDelegate>((long)resizeTableEntry.FunctionPointer, DXGIResizeTargetDelegate);
            //dx11Overlay.direct2DRenderMethod = renderDelegate;

            // Return our DX9Overlay
            return dx11Overlay;
        }

        /// <summary>
        /// Private constructor, please use factory method <see cref="CreateDirectXOverlay"/> instead.
        /// </summary>
        private DX11Overlay() { }

        /*
        /// <summary>
        /// Hook for the IDXGISwapChain.Present function of DirectX 11, used to show the rendered image right to the user.
        /// </summary>
        /// <param name="swapChainPtr">The pointer to the actual swapchain, `this` object.</param>
        /// <param name="syncInterval">An integer that specifies how to synchronize presentation of a frame with the vertical blank.</param>
        /// <param name="flags">An integer value that contains swap-chain presentation options. These options are defined by the DXGI_PRESENT constants.</param>
        /// <returns></returns>
        private int DXGISwapChainPresentHook(IntPtr swapChainPtr, int syncInterval, PresentFlags flags)
        {
            // Setup Direct2D if it is not already setup.
            /*
            if (!_direct2DSetup)
            { SetupDirect2D(swapChainPtr); }
            
            // Call the original function.
            int returnValue = PresentHook.OriginalFunction(swapChainPtr, syncInterval, flags);

            // Render to the screen, calling EndDraw will back again enter this method, 
            // we need to make sure it doesn't run infinitely.
            if (!isDrawing) {
                isDrawing = true;
                direct2DRenderMethod?.Invoke(direct2DRenderTarget);
            }
            isDrawing = false;

            // Return back to original caller.
            return returnValue;
            *//*

            // Call user function.
            return direct2DRenderMethod.Invoke(swapChainPtr, syncInterval, flags);
        }
        */
        /*
        /// <summary>
        /// Sets up Direct2D rendering to be performed by the user.
        /// Uses rendering to bitma.
        /// </summary>
        /// <param name="swapChainPointer">The pointer to the swapchain object used for drawing.</param>
        private void SetupDirect2D2(IntPtr swapChainPointer)
        {
            // Create the D2D Factory which aids with the creation of a WindowRenderTarget object.
            Factory direct2DFactory = new Factory(FactoryType.SingleThreaded);

            // Get the current swapchain as a managed object.
            SwapChain currentSwapChain = new SwapChain(swapChainPointer);

            // Get surface to be drawn.
            Surface dxgiSurface = Surface.FromSwapChain(currentSwapChain, 0);

            direct2DRenderTarget = new RenderTarget
            (
                direct2DFactory,
                dxgiSurface,
                new RenderTargetProperties
                (
                    new PixelFormat(Format.Unknown, AlphaMode.Ignore)
                )
            );
        }*/

        /*
        /// <summary>
        /// Sets up Direct2D rendering to be performed by the user.
        /// </summary>
        /// <param name="swapChainPointer">The pointer to the swapchain object used for drawing.</param>
        private void SetupDirect2D(IntPtr swapChainPointer)
        {
            // Create the D2D Factory which aids with the creation of a WindowRenderTarget object.
            Factory direct2DFactory = new Factory(FactoryType.SingleThreaded);
            IntPtr gameWindowHandle = Bindings.TargetProcess.GetProcessFromReloadedProcess().MainWindowHandle;

            // Retrieve window size of target window.
            Point windowSize = WindowProperties.GetWindowClientSize(gameWindowHandle);

            // Set the render properties!
            HwndRenderTargetProperties direct2DRenderTargetProperties = new HwndRenderTargetProperties
            {
                Hwnd = gameWindowHandle,
                PixelSize = new Size2(windowSize.X, windowSize.Y),
                PresentOptions = PresentOptions.None
            };

            // Assign the Window Render Target
            direct2DRenderTarget = new WindowRenderTarget
            (
                direct2DFactory,
                new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)),
                direct2DRenderTargetProperties
            );

            // D2D is setup
            _direct2DSetup = true;
        }
        */
    }
}
