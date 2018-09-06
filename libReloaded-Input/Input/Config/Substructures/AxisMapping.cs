using Newtonsoft.Json;

namespace Reloaded.Input.Config.Substructures
{
    /// <summary>
    /// Defines all of the axis mappings for the individual custom controller.
    /// Used for mapping internal XInput and DirectInput axis to own custom defined axis.
    /// </summary>
    public class AxisMapping
    {
        [JsonProperty(Required = Required.Default)]
        public AxisMappingEntry LeftStickX = new AxisMappingEntry();

        [JsonProperty(Required = Required.Default)]
        public AxisMappingEntry LeftStickY = new AxisMappingEntry();

        [JsonProperty(Required = Required.Default)]
        public AxisMappingEntry LeftTrigger = new AxisMappingEntry();

        [JsonProperty(Required = Required.Default)]
        public AxisMappingEntry RightStickX = new AxisMappingEntry();

        [JsonProperty(Required = Required.Default)]
        public AxisMappingEntry RightStickY = new AxisMappingEntry();

        [JsonProperty(Required = Required.Default)]
        public AxisMappingEntry RightTrigger = new AxisMappingEntry();
    }
}
