using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SonicHeroes.Native;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SonicHeroes.Overlay
{
    /// <summary>
    /// Base class storing all Direct2D Code used for rendering upon a WindowRenderTarget instance.
    /// Instantiated by providing an appropriate window handle to a window or an overlay window on which content is to be drawn to.
    /// Use this class only if you know what you are doing. If you are looking for the external overlay, look at OverlayGlassDirect2D
    /// </summary>
    public abstract class Direct2DWindowRenderTargetBase
    {
        /// <summary>
        /// Provides a lock mechanism to disallow multiple threads to render or use DirectX rendering functions at once.
        /// Threads wait until the lock is released to execute DirectX drawing code.
        /// </summary>
        static readonly object renderLock = new object();

        /// <summary>
        /// Declares a render target that is used to render to a window surface.
        /// </summary>
        public SharpDX.Direct2D1.WindowRenderTarget Direct2DWindowTarget { get; set; }

        /// <summary>
        /// Delegate instance which declares/stores references to methods rendering visual elements.
        /// In order to render a new element/method to render to the screen, add the individual method to this delegate.
        /// </summary>
        public Delegate_RenderDirect2D Direct2DRenderMethod { get; set; }

        /// <summary>
        /// Delegate instance which declares/stores references to methods executed after rendering of visual elements.
        /// Allows for code to be ran on every rendered frame of the overlay.
        /// </summary>
        public Delegate_OnFrameDelegate Direct2DOnframeDelegate { get; set; }

        /// <summary>
        /// Specifies the handle upon which we will be drawing with Direct2D onto.
        /// </summary>
        public IntPtr TargetWindowHandle { get; set; }

        /// <summary>
        /// True once setup of Direct2D is complete.
        /// </summary>
        public bool Direct2DSetupComplete { get; set; }

        /// <summary>
        /// Defines a delegate signature used for rendering visual elements using direct2D.
        /// </summary>
        /// <param name="direct2DWindowTarget"></param>
        public delegate void Delegate_RenderDirect2D(WindowRenderTarget direct2DWindowTarget);

        /// <summary>
        /// Defines a delegate signature used for running custom code after rendering of the visual elements using direct2D.
        /// </summary>
        public delegate void Delegate_OnFrameDelegate();

        /// <summary>
        /// Provide empty constructor which does not instantiate the object, merely for inheritance purposes.
        /// </summary>
        public Direct2DWindowRenderTargetBase() { }

        /// <summary>
        /// Class constructor. 
        /// </summary>
        /// <param name="targetWindowHandle">Handle to the window which you want to draw ontop of.</param>
        public Direct2DWindowRenderTargetBase(IntPtr targetWindowHandle)
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
            this.TargetWindowHandle = targetWindowHandle;

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
                SharpDX.Direct2D1.Factory direct2DFactory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);

                // Retrieve window size of target window.
                Point windowSize = Windows.GetWindowClientSize(targetWindowHandle);

                // Set the render properties!
                SharpDX.Direct2D1.HwndRenderTargetProperties direct2DRenderTargetProperties = new HwndRenderTargetProperties();
                direct2DRenderTargetProperties.Hwnd = targetWindowHandle;
                direct2DRenderTargetProperties.PixelSize = new SharpDX.Size2(windowSize.X, windowSize.Y);
                direct2DRenderTargetProperties.PresentOptions = PresentOptions.None;

                // Assign the Window Render Target
                Direct2DWindowTarget = new SharpDX.Direct2D1.WindowRenderTarget
                (
                    direct2DFactory,
                    new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)),
                    direct2DRenderTargetProperties
                );

                // Clear the screen of the Window Render Target.
                DirectXClearScreen();

                // Mark D2D Setup as Complete, allow Rendering.
                Direct2DSetupComplete = true;
            }
            catch { }
        }

        /// <summary>
        /// Rendering is handled here, you must first pass a method to Direct2D_Render_Method which will be used for handling the rendering.
        /// </summary>
        public void DirectXRender()
        {
            lock (renderLock)
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
                    try { Direct2DOnframeDelegate?.Invoke(); } catch { } 

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
            lock (renderLock)
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
            Direct2DWindowTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a));
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
            WinAPI.Windows.SetParent(yourFormHandle, TargetWindowHandle);

            // Bring the user's form to front.
            yourForm.BringToFront();
        }
    }
}
