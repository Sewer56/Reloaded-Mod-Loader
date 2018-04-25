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
using System.Drawing;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using WindowProperties = Reloaded.Native.Functions.WindowProperties;

namespace Reloaded.Overlay
{
    /// <summary>
    /// Base class storing all Direct2D Code used for rendering to a window.
    /// Instantiated by providing an appropriate window handle to a window or an overlay window on which content is to be drawn to.
    /// Use this class only if you know what you are doing. If you are looking for the external overlay, look at OverlayGlassDirect2D
    /// </summary>
    public class D2DWindowRenderer
    {
        /// <summary>
        /// Defines a delegate signature used for running custom code after rendering of the visual elements using direct2D.
        /// </summary>
        public delegate void DelegateOnFrameDelegate();

        /// <summary>
        /// Defines a delegate signature used for rendering visual elements using direct2D.
        /// </summary>
        /// <param name="direct2DWindowTarget">Window Render Target used for drawing with Direct2D by the end user.</param>
        public delegate void DelegateRenderDirect2D(WindowRenderTarget direct2DWindowTarget);

        /// <summary>
        /// Declares a render target that is used to render to a window surface.
        /// </summary>
        public WindowRenderTarget Direct2DWindowTarget { get; private set; }

        /// <summary>
        /// Delegate instance which declares/stores references to methods rendering visual elements.
        /// In order to render a new element/method to render to the screen, add the individual method to this delegate.
        /// </summary>
        public DelegateRenderDirect2D Direct2DRenderMethod { get; set; }

        /// <summary>
        /// Delegate instance which declares/stores references to methods executed after rendering of visual elements.
        /// Allows for code to be ran on every rendered frame of the overlay.
        /// </summary>
        public DelegateOnFrameDelegate Direct2DOnframeDelegate { get; set; }

        /// <summary>
        /// True once setup of Direct2D is complete.
        /// </summary>
        public bool Direct2DSetupComplete { get; private set; }

        /// <summary>
        /// Provides a lock mechanism to disallow multiple threads to render or use DirectX rendering functions at once.
        /// Threads wait until the lock is released to execute DirectX drawing code.
        /// </summary>
        private static readonly object RenderLock = new object();

        /// <summary>
        /// Provide empty constructor which does not instantiate the object, merely for inheritance purposes.
        /// </summary>
        public D2DWindowRenderer() { }

        /// <summary>
        /// Class constructor. 
        /// </summary>
        /// <param name="targetWindowHandle">Handle to the window which you want to draw ontop of.</param>
        public D2DWindowRenderer(IntPtr targetWindowHandle)
        {
            // Set whether the setup of D2D is complete.
            Direct2DSetupComplete = false;

            // Initialize Direct2D on a separate thread.
            InitializeDirectX(targetWindowHandle);
        }

        /// <summary>
        /// Class constructor with assignable Render method. 
        /// </summary>
        /// <param name="targetWindowHandle">Handle to the window which you want to draw ontop of.</param>
        /// <param name="d2dRenderMethod">The delegate method used to render onto the screen with Direct2D.</param>
        public D2DWindowRenderer(IntPtr targetWindowHandle, DelegateRenderDirect2D d2dRenderMethod)
        {
            // Set the method.
            Direct2DRenderMethod = d2dRenderMethod;

            // Set whether the setup of D2D is complete.
            Direct2DSetupComplete = false;

            // Initialize Direct2D on a separate thread.
            InitializeDirectX(targetWindowHandle);
        }

        /// <summary>
        /// Resizes the Direct2D window to match the client area size of a specified window handle.
        /// </summary>
        /// <param name="targetWindowHandle">The handle of the window of whose client area size should be matched.</param>
        public void ResizeWindow(IntPtr targetWindowHandle)
        {
            // Wait for any draw operation to finish.
            lock (RenderLock)
            {
                // Retrieve window size of target window.
                Point windowSize = WindowProperties.GetClientAreaSize2(targetWindowHandle);

                // Resize the D2D WindowRenderTarget
                Direct2DWindowTarget.Resize(new Size2(windowSize.X, windowSize.Y));
            }
        }

        /// <summary>
        /// Initializes a Direct2D device used to draw to the screen. 
        /// For drawing, you MUST add your methods to the Direct2D Rendering Delegate (direct2DRenderMethod).
        /// </summary>
        public void InitializeDirectX(IntPtr targetWindowHandle)
        {
            try
            {
                // Wait for any draw operation to finish.
                lock (RenderLock)
                {
                    // Mark D2D Setup as incomplete, disallow Rendering.
                    Direct2DSetupComplete = false;

                    // Dispose Render Target if Necessary
                    Direct2DWindowTarget?.Dispose();

                    // Create the D2D Factory which aids with the creation of a WindowRenderTarget object.
                    Factory direct2DFactory = new Factory(FactoryType.SingleThreaded);

                    // Retrieve window size of target window.
                    Point windowSize = WindowProperties.GetClientAreaSize2(targetWindowHandle);

                    // Set the render properties!
                    HwndRenderTargetProperties direct2DRenderTargetProperties = new HwndRenderTargetProperties
                    {
                        Hwnd = targetWindowHandle,
                        PixelSize = new Size2(windowSize.X, windowSize.Y),
                        PresentOptions = PresentOptions.None
                    };

                    // Assign the Window Render Target
                    Direct2DWindowTarget = new WindowRenderTarget
                    (
                        direct2DFactory,
                        new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)),
                        direct2DRenderTargetProperties
                    );

                    // Clear the screen of the Window Render Target.
                    DirectXClearScreen();

                    // Mark D2D Setup as Complete, allow Rendering.
                    Direct2DSetupComplete = true;
                }
            }
            catch (Exception ex)
            {
                Bindings.PrintError?.Invoke("[libReloaded] Failed to initialize DirectX rendering to window | " + ex.Message);
            }
        }

        /// <summary>
        /// Renders your user contents to the screen.
        /// Before running this, make sure that <see cref="Direct2DRenderMethod"/> is assigned.
        /// </summary>
        public void DirectXRender()
        {
            lock (RenderLock)
            {
                // If the window/surface we are rendering on is correctly set up and ready.
                if (Direct2DSetupComplete)
                {
                    // Begin Drawing!
                    Direct2DWindowTarget.BeginDraw();

                    // Clear everything drawn previously.
                    ClearScreen(0, 0, 0, 0); 

                    // Call our own rendering methods assigned to the delegate.
                    Direct2DRenderMethod?.Invoke(Direct2DWindowTarget);

                    // Run any of our own assigned code to be run after rendering occurs... safely
                    try { Direct2DOnframeDelegate?.Invoke(); }
                    catch (Exception ex)
                    {
                        Bindings.PrintWarning?.Invoke("[libReloaded] Exception thrown in user code ran on Window " +
                                                      "overlay frame render, let the mod/application developer know he screwed up | " + ex.Message);
                    }

                    // End Drawing
                    Direct2DWindowTarget.EndDraw();
                }
            }
        }

        /// <summary>
        /// Draws over the entire form/window with RGBA(0,0,0,0). 
        /// This clears the entire screen of any previously drawn graphics.
        /// </summary>
        public void DirectXClearScreen()
        {
            lock (RenderLock)
            {
                // If the window/surface we are rendering on is correctly set up and ready.
                if (Direct2DSetupComplete)
                {
                    // Begin Drawing!
                    Direct2DWindowTarget.BeginDraw();

                    // Clears everything drawn previously.
                    ClearScreen(0, 0, 0, 0);

                    // End Drawing!
                    Direct2DWindowTarget.EndDraw(); 
                }
            }

        }

        /// <summary>
        /// Clears the drawing area with the specified RGBA Colours. Use this to wipe the screen.
        /// </summary>
        private void ClearScreen(float r, float g, float b, float a)
        {
            Direct2DWindowTarget.Clear(new RawColor4(r, g, b, a));
        }
    }
}
