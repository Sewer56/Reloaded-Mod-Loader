using Newtonsoft.Json;

namespace Reloaded.Input.Config.Substructures
{
    /// <summary>
    /// Custom mapping for the generic buttons. 
    /// Maps each regular button of an XBOX to a button ID custom defined by the user.
    /// Sony Playstation and Nintendo equivalents are provided in comments for each button, 
    /// for the convenience of the programmer.
    /// </summary>
    public class ButtonMapping
    {
        /// <summary>
        /// Initializes the button mapping structure with default values.
        /// </summary>
        public ButtonMapping()
        {
            ButtonA = 255;
            ButtonB = 255;
            ButtonBack = 255;
            ButtonGuide = 255;
            ButtonLb = 255;
            ButtonLs = 255;
            ButtonRb = 255;
            ButtonRs = 255;
            ButtonStart = 255;
            ButtonX = 255;
            ButtonY = 255;
        }

        /// <summary>
        /// Playstation: Cross, Nintendo: B 
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonA;

        /// <summary>
        /// Playstation: Circle, Nintendo: A
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonB;

        /// <summary>
        /// Playstation: Select, Nintendo: Select
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonBack;

        /// <summary>
        /// Playstation: PS Button, Nintendo: Home
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonGuide;

        /// <summary>
        /// Playstation: L1, Nintendo: L
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonLb;

        /// <summary>
        /// Playstation: L3, Nintendo: L Click
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonLs;

        /// <summary>
        /// Playstation: R1, Nintendo: R
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonRb;

        /// <summary>
        /// Playstation: R3, Nintendo: R Click 
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonRs;

        /// <summary>
        /// Playstation: Select, Nintendo: Start
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonStart;

        /// <summary>
        /// Playstation: Square, Nintendo: Y 
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonX;

        /// <summary>
        /// Playstation: Triangle, Nintendo: X
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public byte ButtonY;
    }
}
