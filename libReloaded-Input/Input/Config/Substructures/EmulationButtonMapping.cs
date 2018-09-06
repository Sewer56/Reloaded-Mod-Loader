using Newtonsoft.Json;

namespace Reloaded.Input.Config.Substructures
{
    /// <summary>
    /// Defines a struct specifying a custom mapping of buttons to the DPAD and axis for devices which do not
    /// support analog or POV-hat input such as keyboards and other potential peripherals.
    /// The emulated keys override the real keys if they are set.
    /// </summary>
    public class EmulationButtonMapping
    {
        /// <summary>
        /// Default constructor initializing the default values.
        /// </summary>
        public EmulationButtonMapping()
        {
            DpadDown = 255;
            DpadLeft = 255;
            DpadRight = 255;
            DpadUp = 255;
            LeftStickDown = 255;
            LeftStickLeft = 255;
            LeftStickRight = 255;
            LeftStickUp = 255;
            LeftTrigger = 255;
            RightStickDown = 255;
            RightStickLeft = 255;
            RightStickRight = 255;
            RightStickUp = 255;
            RightTrigger = 255;
        }

        /// <summary>
        /// For keyboards and other misc input devices. Simulates DPAD DOWN if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte DpadDown;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates DPAD LEFT if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte DpadLeft;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates DPAD RIGHT if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte DpadRight;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates DPAD UP if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte DpadUp;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte LeftStickDown;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte LeftStickLeft;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte LeftStickRight;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte LeftStickUp;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left trigger if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte LeftTrigger;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte RightStickDown;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte RightStickLeft;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte RightStickRight;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte RightStickUp;

        /// <summary>
        /// For keyboards and other misc input devices. Simulates right trigger if pressed.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte RightTrigger;
    }
}
