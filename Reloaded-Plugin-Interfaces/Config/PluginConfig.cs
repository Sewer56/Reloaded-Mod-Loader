using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reloaded.Paths;

namespace Reloaded_Plugin_System.Config
{
    /// <summary>
    /// A singular configuration which describes a plugin.
    /// </summary>
    public class PluginConfig
    {
        /* Fields */
        public string Name            { get; set; } = "Plugin Name Undefined";
        public string Description     { get; set; } = "Plugin Description Undefined";
        public string Version         { get; set; } = "Undefined";
        public string Author          { get; set; } = "Undefined";
        public string ConfigFile      { get; set; } = "N/A";
        public string Site            { get; set; } = "N/A";
        public string DllName         { get; set; } = "";
        public bool   Enabled         { get; set; } = false;

        [JsonIgnore]
        public string PluginConfigLocation  { get; set; }

        [JsonIgnore]
        public string ConfigFileLocation => $"{Path.GetDirectoryName(PluginConfigLocation)}//{ConfigFile}";

        /*
            ------------
            Input/Output
            ------------
        */
        public static PluginConfig ParseConfig(string directoryWithConfig)
        {
            string expectedLocation = $"{directoryWithConfig}/{Strings.Parsers.ConfigFile}";
            PluginConfig pluginConfig = new PluginConfig();
            if (File.Exists(expectedLocation))
            {
                try
                {
                    pluginConfig = JsonConvert.DeserializeObject<PluginConfig>(File.ReadAllText(expectedLocation));
                }
                catch { /* Ignored */ }
            }
            
            pluginConfig.PluginConfigLocation = expectedLocation;
            return pluginConfig;
        }

        public static void WriteConfig(PluginConfig pluginConfig)
        {
            string json = JsonConvert.SerializeObject(pluginConfig, Formatting.Indented);
            File.WriteAllText(pluginConfig.PluginConfigLocation, json);
        }

        public static List<PluginConfig> GetAllConfigs()
        {
            // Retrieves the name of all directories in the 'Themes' folder.
            string[] directories = Directory.GetDirectories(LoaderPaths.GetPluginsDirectory());
            List<PluginConfig> pluginConfigs = new List<PluginConfig>(directories.Length);

            foreach (var directory in directories)
            {
                pluginConfigs.Add(ParseConfig(directory));
            }

            return pluginConfigs;
        }

        /*
            --------------
            Business Logic
            --------------
        */

        /// <summary>
        /// Retrieves the name of the Dll to be loaded for this plugin.
        /// Can either be the default "Plugin.dll" or a user set name.
        /// </summary>
        /// <returns></returns>
        public string GetDllName()
        {
            return !String.IsNullOrEmpty(DllName) ? DllName : "Plugin.dll";
        }

        /// <summary>
        /// Returns the absolute path of the DLL file.
        /// </summary>
        public string GetDllPath()
        {
            string pluginDirectory = Path.GetDirectoryName(PluginConfigLocation);
            return Path.Combine(pluginDirectory, GetDllName());
        }
    }
}
