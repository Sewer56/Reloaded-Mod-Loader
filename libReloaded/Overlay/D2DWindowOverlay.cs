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
using System.Windows.Forms;
using Reloaded.Native.WinAPI;
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
    public class D2DWindowOverlay
    {
        /// <summary>
        /// Defines a delegate signature used for running custom code after rendering of the visual elements using direct2D.
        /// </summary>
        public delegate void DelegateOnFrameDelegate();

        /// <summary>
        /// Defines a delegate signature used for rendering visual elements using direct2D.
        /// </summary>
        /// <param name="direct2DWindowTarget"></param>
        public delegate void DelegateRenderDirect2D(WindowRenderTarget direct2DWindowTarget);

        /// <summary>
        /// Declares a render target that is used to render to a window surface.
        /// </summary>
        public WindowRenderTarget Direct2DWindowTarget { get; set; }

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
        /// Specifies the handle upon which we will be drawing with Direct2D onto.
        /// </summary>
        public IntPtr TargetWindowHandle { get; set; }

        /// <summary>
        /// True once setup of Direct2D is complete.
        /// </summary>
        public bool Direct2DSetupComplete { get; set; }

        /// <summary>
        /// Provides a lock mechanism to disallow multiple threads to render or use DirectX rendering functions at once.
        /// Threads wait until the lock is released to execute DirectX drawing code.
        /// </summary>
        private static readonly object RenderLock = new object();

        /// <summary>
        /// Provide empty constructor which does not instantiate the object, merely for inheritance purposes.
        /// </summary>
        public D2DWindowOverlay() { }

        /// <summary>
        /// Class constructor. 
        /// </summary>
        /// <param name="targetWindowHandle">Handle to the window which you want to draw ontop of.</param>
        public D2DWindowOverlay(IntPtr targetWindowHandle)
        {
            // Set whether the setup of D2D is complete.
            Direct2DSetupComplete = false;

            // Redirects the constructor contents to execute in a different location.
            ConstructorAlias(targetWindowHandle);
        }

        /// <summary>
        /// Contents of the constructor for the class, allowing the constructor to be executed with parameters determined after the creation
        /// of any parent class in an inheritance structure.
        /// </summary>
        /// <param name="targetWindowHandle">Handle to the window which you want to draw ontop of.</param>
        public void ConstructorAlias(IntPtr targetWindowHandle)
        {
            // Set handle to which we are drawing on.
            TargetWindowHandle = targetWindowHandle;

            // Initialize Direct2D on a separate thread.
            InitializeDirectX(targetWindowHandle);
        }

        /// <summary>
        /// Initializes a Direct2D device used to draw to the screen. 
        /// For drawing, you MUST add your methods to the Direct2D Rendering Delegate (direct2DRenderMethod).
        /// </summary>
        private void InitializeDirectX(IntPtr targetWindowHandle)
        {
            try
            {
                // Create the D2D Factory which aids with the creation of a WindowRenderTarget object.
                Factory direct2DFactory = new Factory(FactoryType.SingleThreaded);

                // Retrieve window size of target window.
                Point windowSize = WindowProperties.GetWindowClientSize(targetWindowHandle);

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
            catch (Exception ex)
            {
                Bindings.PrintError?.Invoke("[libReloaded] Failed to initialize DirectX rendering to window | " + ex.Message);
            }
        }

        /// <summary>
        /// Rendering is handled here, you must first pass a method to Direct2D_Render_Method which will be used for handling the rendering.
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
                    Direct2DRenderMethod(Direct2DWindowTarget); 

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

        /// <summary>
        /// Attaches your current form to the specified target window.
        /// Makes your form a child form of the overlay form.
        /// Use only for debugging purposes.
        /// </summary>
        /// <param name="yourForm">Your windows form.</param>
        public void AttachWinForm(Form yourForm)
        {
            // Define handle to your own Windows form.
            IntPtr yourFormHandle = yourForm.Handle;

            // Set parent form.
            WindowFunctions.SetParent(yourFormHandle, TargetWindowHandle);

            // Bring the user's form to front.
            yourForm.BringToFront();
        }
    }
}
