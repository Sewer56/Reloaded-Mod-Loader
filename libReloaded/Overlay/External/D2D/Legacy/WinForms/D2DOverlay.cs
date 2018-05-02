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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using Reloaded.Native.WinAPI;
using Reloaded.Overlay.External.WinForms;
using Reloaded.Overlay.Modules;

namespace Reloaded.Overlay.External.D2D
{
    /// <summary>
    /// This class is responsible for allowing you to instantiate a Windows Forms overlay which will be drawn ontop of a game
    /// or chosen window, providing the target window isn't in exclusive fullscreen mode.
    /// </summary>
    public class D2DOverlay
    {
        /// <summary>
        /// Fake glass form which we will be overlaying the game with.
        /// </summary>
        public TransparentWinform OverlayForm { get; set; }

        /// <summary>
        /// A thread which hosts the glass overlay windows form, ensuring that it keeps running.
        /// </summary>
        public Thread WPFThread { get; private set; }

        /// <summary>
        /// A thread which hosts the render loop, automatically calling your Direct2D Render Delegate
        /// and rendering items to the screen.
        /// </summary>
        public Thread RenderLoopThread { get; private set; }

        /// <summary>
        /// An instance of the Direct2D Window Renderer used to draw ontop
        /// of our invisible Windows Form.
        /// </summary>
        public D2DWindowRenderer DirectD2DWindowRenderer { get; set; }

        /// <summary>
        /// This is kept only to ensure no garbage collection.
        /// </summary>
        private System.Windows.Application _windowsApplication;

        /// <summary>
        /// Defines the amount of time of sleep for the CPU, rounded down to the nearest millisecond.
        /// </summary>
        private float FrameRate { get; set; }

        /// <summary>
        /// Defines the stopwatch used for the accurate measurement of frames.
        /// </summary>
        private Stopwatch StopWatch { get; set; }

        /// <summary>
        /// Empty private constructor, use factory method.
        /// See <see cref="CreateExternalWindowOverlay"/>
        /// </summary>
        private D2DOverlay() { }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// </summary>
        /// <param name="gameWindowName">
        ///     The name of the window to be overlayed.
        ///     The window may be the game window or another window elsewhere.
        /// </param>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <returns>
        ///     1. True if the window has been found and overlay has been instantiated, else false.
        ///     2. An instance of self (ExternalWindowOverlay) with the overlay enabled and running.
        /// </returns>
        public static async Task<(bool success, D2DOverlay overlay)> CreateExternalWindowOverlay(string gameWindowName, D2DWindowRenderer.DelegateRenderDirect2D renderDelegate)
        {
            return await CreateExternalWindowOverlay(gameWindowName, renderDelegate, 0);
        }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// </summary>
        /// <param name="gameWindowName">
        ///     The name of the window to be overlayed.
        ///     The window may be the game window or another window elsewhere.
        /// </param>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <param name="hookDelay">
        ///     Specifies the amount of time to wait until the hook is instantiation begins.
        ///     Some games are known to crash if DirectX is hooked too early.
        /// </param>
        /// <returns>
        ///     1. True if the window has been found and overlay has been instantiated, else false.
        ///     2. An instance of self (ExternalWindowOverlay) with the overlay enabled and running.
        /// </returns>
        public static async Task<(bool success, D2DOverlay overlay)> CreateExternalWindowOverlay(string gameWindowName, D2DWindowRenderer.DelegateRenderDirect2D renderDelegate, int hookDelay)
        {
            // Apply hook delay
            await Task.Delay(hookDelay);

            // Create self.
            D2DOverlay externalWindowOverlay = new D2DOverlay();

            // Wait for and find the game window.
            IntPtr windowHandle = await FindWindowHandleByName(gameWindowName);
            
            // If the handle has been acquired.
            if (windowHandle != (IntPtr)0)
            {
                // Wait for the Window to show itself to screen before configuring.
                while (WindowFunctions.IsWindowVisible(windowHandle) == false)
                    await Task.Delay(32);

                // Enable the overlay.
                externalWindowOverlay.EnableOverlay(renderDelegate, windowHandle);

                // Returns true.
                return (true, externalWindowOverlay);
            }

            // Return false, window was not found.
            return (false, externalWindowOverlay);
        }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="gameWindowHandle">
        ///     The handle of the game window to be overlayed.
        ///     The handle may be obtained via ReloadedProcess.GetProcessFromReloadedProcess().MainWindowHandle.
        /// </param>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <returns>An instance of self (ExternalWindowOverlay) with the overlay enabled and running.</returns>
        public static async Task<D2DOverlay> CreateExternalWindowOverlay(IntPtr gameWindowHandle, D2DWindowRenderer.DelegateRenderDirect2D renderDelegate)
        {
            return await CreateExternalWindowOverlay(gameWindowHandle, renderDelegate, 0);
        }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="gameWindowHandle">
        ///     The handle of the game window to be overlayed.
        ///     The handle may be obtained via ReloadedProcess.GetProcessFromReloadedProcess().MainWindowHandle.
        /// </param>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <param name="hookDelay">
        ///     Specifies the amount of time to wait until the hook is instantiation begins.
        ///     Some games are known to crash if DirectX is hooked too early.
        /// </param>
        /// <returns>An instance of self (ExternalWindowOverlay) with the overlay enabled and running.</returns>
        public static async Task<D2DOverlay> CreateExternalWindowOverlay(IntPtr gameWindowHandle, D2DWindowRenderer.DelegateRenderDirect2D renderDelegate, int hookDelay)
        {
            // Apply hook delay
            await Task.Delay(hookDelay);

            // Wait for the Window to show itself to screen before configuring.
            while (WindowFunctions.IsWindowVisible(gameWindowHandle) == false)
                await Task.Delay(32);

            // Create self.
            D2DOverlay externalWindowOverlay = new D2DOverlay();

            // Enable the overlay.
            externalWindowOverlay.EnableOverlay(renderDelegate, gameWindowHandle);

            // Return self
            return externalWindowOverlay;
        }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <returns>An instance of self (ExternalWindowOverlay) with the overlay enabled and running.</returns>
        public static async Task<D2DOverlay> CreateExternalWindowOverlay(D2DWindowRenderer.DelegateRenderDirect2D renderDelegate)
        {
            return await CreateExternalWindowOverlay(renderDelegate, 0);
        }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// Note: This method is blocking and Reloaded mods are required to return in order 
        /// to boot up the games, please do not assign this statically - instead assign it in a background thread!
        /// </summary>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <param name="hookDelay">
        ///     Specifies the amount of time to wait until the hook is instantiation begins.
        ///     Some games are known to crash if DirectX is hooked too early.
        /// </param>
        /// <returns>An instance of self (ExternalWindowOverlay) with the overlay enabled and running.</returns>
        public static async Task<D2DOverlay> CreateExternalWindowOverlay(D2DWindowRenderer.DelegateRenderDirect2D renderDelegate, int hookDelay)
        {
            // Apply hook delay
            await Task.Delay(hookDelay);

            // Game process
            System.Diagnostics.Process gameProcess = Bindings.TargetProcess.GetProcessFromReloadedProcess();

            // Try to get game window handle.
            while (gameProcess.MainWindowHandle == IntPtr.Zero)
                await Task.Delay(32);

            // Wait for the Window to show itself to screen before configuring.
            while (WindowFunctions.IsWindowVisible(gameProcess.MainWindowHandle) == false)
                await Task.Delay(32);

            // Create self.
            D2DOverlay externalWindowOverlay = new D2DOverlay();

            // Enable the overlay.
            externalWindowOverlay.EnableOverlay(renderDelegate, gameProcess.MainWindowHandle);

            // Return self
            return externalWindowOverlay;
        }

        /// <summary>
        /// Calls Application.Run to host the overlay glass window such that it may be displayed.
        /// Want multiple windows (for some reason?). Create a thread and call this method.
        /// </summary>
        /// <param name="renderDelegate">
        ///     The user delegate which you may use for rendering to the external overlay 
        ///     window with Direct2D. 
        /// </param>
        /// <param name="targetWindowHandle">
        ///     The handle of the window to display the overlay on.
        /// </param>
        private void EnableOverlay(D2DWindowRenderer.DelegateRenderDirect2D renderDelegate, IntPtr targetWindowHandle)
        {
            // Set framerate
            FrameRate = 60;

            // Initiate the timer
            StopWatch = new Stopwatch();
            StopWatch.Start();

            // Enable the Overlay Window.
            WPFThread = new Thread (() =>
            {
                // Instantiate glass form
                OverlayForm = new TransparentWinform(targetWindowHandle);

                // Create D2D Window Renderer
                DirectD2DWindowRenderer = new D2DWindowRenderer(OverlayForm.Handle, renderDelegate);

                // Setup hook for when window resizes.
                OverlayForm.GameWindowResizeDelegate += () => DirectD2DWindowRenderer.ResizeWindow(OverlayForm.Handle);

                // Run the form in this thread.
                Application.Run(OverlayForm);
            } );
            
            // Enable the rendering loop.
            RenderLoopThread = new Thread
            (
                () =>
                {
                    while (true)
                    {
                        DirectD2DWindowRenderer?.DirectXRender();
                        SleepFrameRate();
                    }
                }
            );
            WPFThread.SetApartmentState(ApartmentState.STA);
            WPFThread.Start();
            RenderLoopThread.Start();
        }

        /// <summary>
        /// Sleeps and then spins a set amount of time to match the target framerate.
        /// </summary>
        public void SleepFrameRate()
        {
            // Calculate time for 1 frame.
            float sleepMilliseconds = 1000F / FrameRate;

            // Calculate time to Thread.Sleep (round down to nearest millisecond)
            int threadSleepMilliseconds = (int)sleepMilliseconds;

            // Go down another millisecond if not 0.
            if (threadSleepMilliseconds >= 1) { threadSleepMilliseconds -= 1; }

            // Sleep the thread.
            Thread.Sleep(threadSleepMilliseconds);

            // Stall with while loop
            while (StopWatch.Elapsed.TotalMilliseconds < sleepMilliseconds)
            { }

            // Reset stopwatch.
            StopWatch.Restart();
        }

        /// <summary>
        /// Waits for an instance of the game window to spawn (searches window by name).
        /// Proceeds to wait until said window is visible to the end user.
        /// </summary>
        /// <param name="gameWindowName">The name of the game window to find the handle for.</param>
        private static async Task<IntPtr> FindWindowHandleByName(string gameWindowName)
        {
            // Provide our own interactive timeout.
            int attempts = 0;
            int threadSleepTime = 16;
            int timeoutMilliseconds = 1000;

            // Handle of the game window
            IntPtr windowHandle = (IntPtr)0;

            // Wait for Game Window to spawn.
            while (true)
            {
                // Increase attempts.
                attempts++;

                // Get the handle for the Sonic_Heroes Window
                windowHandle = WindowFunctions.FindWindow(null, gameWindowName);

                // If handle successfully acquired.
                if (windowHandle != (IntPtr)0) return windowHandle;

                // Check timeout
                if (attempts > timeoutMilliseconds / threadSleepTime) return (IntPtr) 0;

                // Sleep to reduce CPU load.
                await Task.Delay(threadSleepTime);
            }
        }

        /// <summary>
        /// Attaches a Windows Forms window to the overlay.
        /// </summary>
        public static void AttachWinForm(IntPtr yourFormHandle, IntPtr targetFormHandle)
        {
            // Set parent form.
            WindowFunctions.SetParent(yourFormHandle, targetFormHandle);

            // Bring the user's form to front.
            WindowFunctions.SetForegroundWindow(yourFormHandle);
        }
    }
}
