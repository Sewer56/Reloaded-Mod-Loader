using System.IO;
using Newtonsoft.Json;
using Reloaded.Input.Common;
using Reloaded.Input.Config.Substructures;

namespace Reloaded.Input.Config
{
    /// <summary>
    /// Defines a general class which contains the individual input mappings for the remapper.
    /// Separated into a separate class to allow for easier serialization and deserialization.
    /// </summary>
    public class InputMappings
    {
        public InputMappings()
        {
            ButtonMapping = new ButtonMapping();
            AxisMapping = new AxisMapping();
            ControllerId = 0;
            EmulationMapping = new EmulationButtonMapping();
        }

        /// <summary>
        /// Stores the individual button mappings structure for this controller.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public ButtonMapping ButtonMapping;

        /// <summary>
        /// Stores the individual axis mappings structure for this controller.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public AxisMapping AxisMapping;

        /// <summary>
        /// Defines the individual port used for this specific controller.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public int ControllerId;

        /// <summary>
        /// Defines the custom botton mapping which simulates the individual axis and analog inputs.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public EmulationButtonMapping EmulationMapping;

        /// <summary>
        /// Parses a Mod Loader Controller Configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public static IController ParseConfig(string configLocation, IController controller)
        {
            // Store input mappings before parsing
            InputMappings inputMappings;

            // Try parsing input mappings.
            try
            {
                inputMappings =
                    File.Exists(configLocation)
                        ? JsonConvert.DeserializeObject<InputMappings>(File.ReadAllText(configLocation))
                        : new InputMappings();
            }
            catch { inputMappings = new InputMappings(); }

            // Reassign input mappings.
            controller.InputMappings = inputMappings;

            // Return controller.
            return controller;
        }

        /// <summary>
        /// Writes a mod loader controller configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public static void WriteConfig(string configLocation, IController controller)
        {
            // Create path for config if not exists.
            if (!Directory.Exists(Path.GetDirectoryName(configLocation)))
            { Directory.CreateDirectory(Path.GetDirectoryName(configLocation)); }

            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(controller.InputMappings, Formatting.Indented);

            // Write to disk
            File.WriteAllText(configLocation, json);

        }
    }
}
