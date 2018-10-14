using Newtonsoft.Json;
using Reloaded.Input.Common;

namespace Reloaded.Input.Config.Substructures
{
    /// <summary>
    /// Defines an individual mapping entry for a controller axis as defined in Controller_Axis_Struct.
    /// Serves as a bridge to provide each axis with an individual name and misc properties.
    /// </summary>
    public class AxisMappingEntry
    {
        /// <summary>
        /// Defines the mapping for the individual axis entry.
        /// Also sometimes known in the source code as the Destination Axis.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public ControllerCommon.ControllerAxis DestinationAxis { get; set; } = ControllerCommon.ControllerAxis.Null;

        /// <summary>
        /// Defines a deadzone between 0 and 100%. Range: 0-100
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public float DeadZone { get; set; } = 0.05F;

        /// <summary>
        /// True if the axis is to be reversed when being read.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public bool IsReversed { get; set; } = false;

        /// <summary>
        /// Stores the name of the property (DirectInput, XInput) that is mapped to the axis type.
        /// Sometimes known in the source code as PropertyName.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string SourceAxis { get; set; } = "Null";

        /// <summary>
        /// Scales the raw input values by this value.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public float RadiusScale { get; set; } = 1.00F;
    }
}
