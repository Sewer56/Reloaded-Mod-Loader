using System;
using System.Threading;
using System.Windows;
using Reloaded.Native.Functions;
using Reloaded.Overlay.External.D2D;
using Reloaded.Process;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Point = System.Drawing.Point;
using TextAlignment = SharpDX.DirectWrite.TextAlignment;

namespace Reloaded_Mod_Template
{
    public static class Program
    {
        #region Reloaded Mod Template Stuff
        /// <summary>
        /// Holds the game process for us to manipulate.
        /// Allows you to read/write memory, perform pattern scans, etc.
        /// See libReloaded/GameProcess (folder)
        /// </summary>
        public static ReloadedProcess GameProcess;

        /// <summary>
        /// Stores the absolute executable location of the currently executing game or process.
        /// </summary>
        public static string ExecutingGameLocation;

        /// <summary>
        /// Specifies the full directory location that the current mod 
        /// is contained in.
        /// </summary>
        public static string ModDirectory;
        #endregion Reloaded Mod Template Stuff

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static async void Init()
        {
            /*
                Reloaded Mod Loader Sample: Direct2D-WPF Hybrid External Overlay Example
                Architectures supported: X86, X64

                An example of a simple Direct2D based External Overlay that draws
                ontop of the game's window.

                Now improved to be hosted in a WPF window, granting you the full power of 
                WPF combined with Direct2D, and also fixing a long standing Microsoft Windows 10 bug
                that was hard to pin and affected arbitrary configurations.

                WPFOverlayWindow was copied from libReloaded's source, the main development library for
                Reloaded Mod Loader.
            */

            /*
                * Generates a random sleep value ontop of the existing sleep value.
                * We want to reduce the amount of possible collisions of WPF overlays launching
                * at the same identical time, as Application.Current is not updated in real-time (seemingly).
            */
            Random randomNumberGenerator = new Random();
            int randomSleepValue = randomNumberGenerator.Next(0, 3000);
            Thread.Sleep(32); // Ensures different tick value for seeding, offsets overlay launch time to prevent collisions.

            Thread D2DWindowThread = new Thread(() =>
            {
                // Wait for game window.
                while (GameProcess.Process.MainWindowHandle == IntPtr.Zero)
                { Thread.Sleep(1000); }

                // Sleep to not access the main window.
                // Fixes odd badly programmed games like Sonic Adventure 2
                Thread.Sleep(randomSleepValue);

                /* Adjusted and tested for interop between different windows. */

                // Another overlay already active.
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _externalOverlayWindow = new WPFOverlayWindow(GameProcess.Process.MainWindowHandle, RenderDelegate);
                        _externalOverlayWindow.GameWindowResizeDelegate += GameWindowResizeDelegate;
                        _externalOverlayWindow.Show();
                    });
                }
                // This is the first overlay.
                else
                {
                    try
                    {
                        // Create new Application Context.
                        Application WPFApp = new System.Windows.Application();

                        // Create our window
                        _externalOverlayWindow = new WPFOverlayWindow(GameProcess.Process.MainWindowHandle, RenderDelegate);
                        _externalOverlayWindow.GameWindowResizeDelegate += GameWindowResizeDelegate;

                        // Instance the overlay.
                        WPFApp.Run(_externalOverlayWindow);
                    }
                    catch
                    {
                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            _externalOverlayWindow = new WPFOverlayWindow(GameProcess.Process.MainWindowHandle, RenderDelegate);
                            _externalOverlayWindow.GameWindowResizeDelegate += GameWindowResizeDelegate;
                            _externalOverlayWindow.Show();
                        });
                    }
                }
            });
            D2DWindowThread.SetApartmentState(ApartmentState.STA);
            D2DWindowThread.Start();
        }

        /// <summary>
        /// Defines the external Direct2D Window Overlay.
        /// </summary>
        private static WPFOverlayWindow _externalOverlayWindow;

        // Boolean used to initialize properties.
        private static bool _initialized;

        // Some colours.
        private static RawColor4 _transparentWhiteColor;
        private static RawColor4 _transparentPink; 
        private static RawColor4 _transparentRed;
        private static RawColor4 _transparentPurple; 
        private static RawColor4 _transparentDeepPurple;

        // Drawing objects.
        private static SolidColorBrush _squareBrush;
        private static LinearGradientBrush _squareGradientBrush;
        private static RadialGradientBrush _squareRadialGradientBrush;

        // Define some rectangles.
        private static RawRectangleF _boringRectangle = new RawRectangleF(100,100,200,200);
        private static RawRectangleF _gradientRectangle = new RawRectangleF(300, 100, 400, 200);
        private static RawRectangleF _radialRectangle = new RawRectangleF(500, 100, 600, 200);
        private static RawRectangleF _textRectangle = new RawRectangleF(100, 250, 600, 300);

        // Text stuff
        private static TextFormat _textFormat;

        // Gradient stuff
        private static GradientStop[] _gradientStops;

        /// <summary>
        /// Called upon the resizing of the game window.
        /// </summary>
        private static void GameWindowResizeDelegate()
        {
            // Retrieve window size of target window.
            Point windowSize = WindowProperties.GetClientAreaSize2(GameProcess.Process.MainWindowHandle);

            // Resize and/or move elements or needed.
            // Implementation of this is up to you.

            // Well, except this, because the old factory for _textFormat will become outdated and will throw a nice fat exception.
            _initialized = false; 
        }

        /// <summary>
        /// This is our drawing function. In here we can perform Direct2D based drawing.
        /// </summary>
        /// <param name="direct2DWindowTarget">Use this object to draw!</param>
        private static void RenderDelegate(RenderTarget direct2DWindowTarget)
        {

            // Initialize brushes, etc. if not initialized.
            if (!_initialized)
                InitializeProperties(direct2DWindowTarget);

            // Animate the brush gradients.
            AnimateBrushGradient(direct2DWindowTarget);

            // Render some squares.
            direct2DWindowTarget.FillRectangle(_boringRectangle, _squareBrush);
            direct2DWindowTarget.FillRectangle(_gradientRectangle, _squareGradientBrush);
            direct2DWindowTarget.FillRectangle(_radialRectangle, _squareRadialGradientBrush);
            direct2DWindowTarget.DrawText("Direct2D-WPF Hybrid Overlay Demo", _textFormat, _textRectangle, _squareBrush);
        }

        /// <summary>
        /// Used to initialize GUI elements such as Brushes, Fonts and other various
        /// reusable properties.
        /// </summary>
        /// <param name="direct2DWindowTarget">The object we use to draw! Our device! Our soul!</param>
        private static void InitializeProperties(RenderTarget direct2DWindowTarget)
        {
            // Define a few colours.
            _transparentWhiteColor = new RawColor4(1.0F, 1.0F, 1.0F, 0.5F);
            _transparentPink = new RawColor4(0.85F, 0.11F, 0.38F, 0.7F);
            _transparentRed = new RawColor4(0.99F, 0.85F, 0.21F, 0.7F);
            _transparentPurple = new RawColor4(0.56F, 0.14F, 0.67F, 0.7F);
            _transparentDeepPurple = new RawColor4(0.37F, 0.21F, 0.69F, 0.7F);

            // Let's create a boring brush.
            _squareBrush = new SolidColorBrush(direct2DWindowTarget, _transparentWhiteColor);

            // Initialize the Gradient Stops.
            _gradientStops = new[]
            {
                new GradientStop() { Color = _transparentRed,           Position = 0.00F},
                new GradientStop() { Color = _transparentPink,          Position = 0.25F},
                new GradientStop() { Color = _transparentPurple,        Position = 0.50F},
                new GradientStop() { Color = _transparentDeepPurple,    Position = 0.75F},
                new GradientStop() { Color = _transparentRed,           Position = 1.00F}
            };

            /* Text Properties */
            _textFormat?.Dispose();
            _textFormat = new TextFormat(new SharpDX.DirectWrite.Factory(), "Times New Roman", 20);
            _textFormat.TextAlignment = TextAlignment.Center;

            _initialized = true;
        }

        /// <summary>
        /// Creates new brushes in order to perform animations on the individual gradient based squares.
        /// </summary>
        /// <param name="direct2DWindowTarget">The object we use to draw! Our device! Our soul!</param>
        private static void AnimateBrushGradient(RenderTarget direct2DWindowTarget)
        {
            // Animate the gradientstops.
            for (int x = 0; x < _gradientStops.Length; x++)
            {
                float currentPosition = _gradientStops[x].Position;

                if (currentPosition >= 1F)
                    currentPosition = 0F;
                else
                    currentPosition += 0.01F;

                _gradientStops[x].Position = currentPosition;
            }

            // Conditionally dispose.
            _squareGradientBrush?.Factory?.Dispose();
            _squareGradientBrush?.GradientStopCollection?.Dispose();
            _squareGradientBrush?.Dispose();
            _squareRadialGradientBrush?.Factory?.Dispose();
            _squareRadialGradientBrush?.GradientStopCollection?.Dispose();
            _squareRadialGradientBrush?.Dispose();

            // Fix memory leak with GradientStopCollection
            // Thanks, anonymous StackOverflow user! https://stackoverflow.com/questions/47876001/mysterious-direct2d-memory-leak-with-gradients
            using (var gradientStopCollection = new GradientStopCollection(direct2DWindowTarget, _gradientStops))
            {
                /* How about a brush with a gradient? */
                LinearGradientBrushProperties linearGradientBrushProperties = new LinearGradientBrushProperties()
                {
                    StartPoint = new RawVector2(_gradientRectangle.Top - 50, _gradientRectangle.Left - 50),      // Note: Physical location of the start point, in this case top left corner.
                    EndPoint = new RawVector2(_gradientRectangle.Bottom + 50, _gradientRectangle.Right + 50)     // Note: Physical location of the start point, in this case bottom right corner.
                };
                _squareGradientBrush = new LinearGradientBrush(direct2DWindowTarget, linearGradientBrushProperties, gradientStopCollection);

                /* A radial brush? */
                RadialGradientBrushProperties radialGradientBrushProperties = new RadialGradientBrushProperties()
                {
                    Center = new RawVector2(_radialRectangle.Left + 50, _radialRectangle.Top + 50),    // Note: Physical location of the start point, in this case the actual center.
                    GradientOriginOffset = new RawVector2(0, 0), 
                    RadiusX = 100,
                    RadiusY = 100
                };
                _squareRadialGradientBrush = new RadialGradientBrush(direct2DWindowTarget, radialGradientBrushProperties, gradientStopCollection);
            }
        }
    }
}
